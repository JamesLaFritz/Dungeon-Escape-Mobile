using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField, Tag] private string m_PlayerTag;

    [SerializeField] private BoolReference m_playerInShop;

    [SerializeField] private ShopVariable m_currentShop;

    public ShopItem[] shopItems = new ShopItem[3];

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(m_PlayerTag)) return;

        m_currentShop.SetValue(this);
        //Debug.Log(m_currentShop.Value);
        m_playerInShop.Value = true;
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
    /// This function can be a coroutine.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        m_playerInShop.Value = false;
    }
}
