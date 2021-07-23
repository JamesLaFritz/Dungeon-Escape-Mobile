using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private IntReference m_playerGems;

    [SerializeField] private Inventory m_playerInventory;
    private bool m_hasInventory;

    [SerializeField] private ShopVariable m_currentShop;
    private bool m_hasCurrentShop;

    [SerializeField] private ShopItem m_currentItem;

    [SerializeField] private Text m_shopGemsText;
    private bool m_hasGemsText;

    [SerializeField] private Transform m_itemPanel;

    [SerializeField] private GameObject m_shopItemUIPrefab;
    private bool m_hasShopItemUIPrefab;

    private List<ShopItemUI> m_shopItemUis = new List<ShopItemUI>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (m_itemPanel == null)
        {
            Debug.LogError(
                "Shop UI needs an Item Panel to place the Items the shops sells. Please assign one in the Inspector!",
                gameObject);
            Destroy(this);
            return;
        }

        m_hasGemsText = m_shopGemsText != null;
        m_hasShopItemUIPrefab = m_shopItemUIPrefab != null;
        m_hasCurrentShop = m_currentShop != null;
        m_hasInventory = m_playerInventory != null;

        RemoveItems();
    }

    private void OnEnable()
    {
        UpdateShopGemsText();
        AddItems();
    }

    private void OnDisable()
    {
        RemoveItems();
    }

    private void Update()
    {
        foreach (ShopItemUI shopItemUi in m_shopItemUis!.Where(shopItemUi => shopItemUi is { }))
        {
            System.Diagnostics.Debug.Assert(shopItemUi != null, nameof(shopItemUi) + " != null");
            shopItemUi.DeselectItem();
        }

        UpdateShopGemsText();
    }

    private void RemoveItems()
    {
        System.Diagnostics.Debug.Assert(m_itemPanel != null, nameof(m_itemPanel) + " != null");
        if (!(m_itemPanel.childCount > 0)) return;

        for (int i = m_itemPanel.childCount - 1; i > -1; i--)
        {
            Destroy(m_itemPanel.GetChild(i)?.gameObject);
        }
    }

    private void AddItems()
    {
        if (!m_hasCurrentShop || !m_hasShopItemUIPrefab) return;
        System.Diagnostics.Debug.Assert(m_currentShop != null, nameof(m_currentShop) + " != null");
        if (m_currentShop.Value == null || m_currentShop.Value.shopItems == null) return;

        ShopItem[] shopItems = m_currentShop.Value.shopItems;
        m_shopItemUis = new List<ShopItemUI>();
        int firstSelectedIndex = 0;

        for (int i = 0; i < shopItems.Length && i < 3; i++)
        {
            if (shopItems[i] == null)
            {
                if (i == firstSelectedIndex) firstSelectedIndex++;
                continue;
            }

            if (m_hasInventory)
            {
                if (m_playerInventory.ContainsItem(shopItems[i].gameItem))
                {
                    if (firstSelectedIndex == i) firstSelectedIndex++;
                    continue;
                }
            }

            ShopItemUI ui = Instantiate(m_shopItemUIPrefab, m_itemPanel)?.GetComponent<ShopItemUI>();
            if (ui == null) continue;

            ui.SetItem(shopItems[i]);
            m_shopItemUis.Add(ui);
            if (i == firstSelectedIndex)
                ui.SelectItem();
        }
    }

    private void UpdateShopGemsText()
    {
        if (!m_hasGemsText) return;

        System.Diagnostics.Debug.Assert(m_shopGemsText != null, nameof(m_shopGemsText) + " != null");
        System.Diagnostics.Debug.Assert(m_playerGems != null, nameof(m_playerGems) + " != null");
        m_shopGemsText.text = $"{m_playerGems.Value} G";
    }

    public void OnBuyItemClicked()
    {
        if (m_playerGems == null || m_currentItem == null) return;

        if (m_playerGems.Value >= m_currentItem.cost)
        {
            // subtract cost from player gems
            m_playerGems.Value -= m_currentItem.cost;

            System.Diagnostics.Debug.Assert(UIManager.Instance != null, "UIManager.Instance != null");
            UIManager.Instance.DisplayMessage($"Purchased {m_currentItem.gameItem.name}");

            // add the item to the inventory.
            if (!m_hasInventory) return;
            System.Diagnostics.Debug.Assert(m_playerInventory != null, nameof(m_playerInventory) + " != null");
            m_playerInventory.Add(m_currentItem.gameItem);

            RemoveItemFromShop();
        }
        else
        {
            System.Diagnostics.Debug.Assert(UIManager.Instance != null, "UIManager.Instance != null");
            UIManager.Instance.DisplayMessage("Not Enough Gems.");
        }
    }

    private void RemoveItemFromShop()
    {
        foreach (ShopItemUI shopItemUi in m_shopItemUis!.Where(shopItemUi => shopItemUi is {IsItemCurrentSelected: true}))
        {
            shopItemUi.DisableItem();
            m_shopItemUis.Remove(shopItemUi);
            Destroy(shopItemUi.gameObject);
            return;
        }
    }
}
