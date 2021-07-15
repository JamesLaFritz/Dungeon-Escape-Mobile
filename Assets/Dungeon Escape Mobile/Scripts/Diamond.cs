using UnityEngine;

/// <summary>
/// Allow Player to Collect it in On Trigger Enter.
/// Add the Value to the Player Diamonds.
/// </summary>
public class Diamond : MonoBehaviour
{
    [SerializeField] private int m_value;
    [SerializeField] private IntReference m_playerDiamonds;
    [SerializeField, Tag] private string m_collectorTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(m_collectorTag)) return;

        m_playerDiamonds.Value += m_value;

        Destroy(gameObject);
    }
}
