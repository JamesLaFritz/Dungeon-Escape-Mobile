using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Button m_itemButton;
    private bool m_hasItemButton;
    [SerializeField] private Text m_itemText;
    private bool m_hasItemText;
    [SerializeField] private Text m_costText;
    private bool m_hasCostText;
    [SerializeField] private Image m_selectionImage;
    private bool m_hasSelectionImage;

    [SerializeField] private ShopItem m_currentItem;
    private ShopItem m_shopItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_hasItemButton = m_itemButton != null;
        m_hasItemText = m_itemText != null;
        m_hasCostText = m_costText != null;
        m_hasSelectionImage = m_selectionImage != null;
    }

    public void SelectItem()
    {
        //Debug.Log($"Selected {name}");

        m_currentItem.cost = m_shopItem.cost;
        m_currentItem.gameItem = m_shopItem.gameItem;
        //m_currentItem = m_shopItem;

        if (!m_hasSelectionImage) return;
        System.Diagnostics.Debug.Assert(m_selectionImage != null, nameof(m_selectionImage) + " != null");
        m_selectionImage.gameObject.SetActive(true);
    }

    public void DeselectItem()
    {
        if (!m_hasSelectionImage) return;

        if (m_currentItem.cost == m_shopItem.cost && m_currentItem.gameItem == m_shopItem.gameItem) return;

        System.Diagnostics.Debug.Assert(m_selectionImage != null, nameof(m_selectionImage) + " != null");
        m_selectionImage.gameObject.SetActive(false);
    }

    private void SetCost()
    {
        if (!m_hasCostText) return;

        System.Diagnostics.Debug.Assert(m_costText != null, nameof(m_costText) + " != null");
        m_costText.text = $"{m_shopItem.cost}G";
    }

    private void SetItemText()
    {
        if (!m_hasItemText) return;

        System.Diagnostics.Debug.Assert(m_itemText != null, nameof(m_itemText) + " != null");
        m_itemText.text = m_shopItem.gameItem.name;
    }

    public void SetItem([NotNull] ShopItem item)
    {
        m_shopItem = item;
        name = m_shopItem.name;
        SetCost();
        SetItemText();
    }
}
