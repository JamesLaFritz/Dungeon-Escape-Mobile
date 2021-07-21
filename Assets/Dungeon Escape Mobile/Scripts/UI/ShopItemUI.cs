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
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    ///
    /// This function can be a coroutine.
    /// </summary>
    private void Start()
    {
        m_hasItemButton = m_itemButton != null;
        m_hasItemText = m_itemText != null;
        m_hasCostText = m_costText != null;
        m_hasSelectionImage = m_selectionImage != null;
    }

    public void SelectItem()
    {
        Debug.Log($"Selected {name}");

        if (!m_hasSelectionImage) return;
        System.Diagnostics.Debug.Assert(m_selectionImage != null, nameof(m_selectionImage) + " != null");
        m_selectionImage.gameObject.SetActive(true);

        m_currentItem = m_shopItem;
    }

    public void DeselectItem()
    {
        if (!m_hasSelectionImage) return;

        System.Diagnostics.Debug.Assert(m_selectionImage != null, nameof(m_selectionImage) + " != null");
        m_selectionImage.gameObject.SetActive(false);
    }

    private void SetCost(int cost)
    {
        if (!m_hasCostText) return;

        System.Diagnostics.Debug.Assert(m_costText != null, nameof(m_costText) + " != null");
        m_costText.text = $"{cost}G";
    }

    private void SetItemText(string itemText)
    {
        if (!m_hasItemText) return;

        System.Diagnostics.Debug.Assert(m_itemText != null, nameof(m_itemText) + " != null");
        m_itemText.text = itemText;
    }

    public void SetItem([NotNull] ShopItem item)
    {
        m_shopItem = item;
        SetCost(item.cost);
        SetItemText(item.gameItem.name);
    }
}
