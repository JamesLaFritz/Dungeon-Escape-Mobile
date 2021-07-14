using UnityEngine;

/// <summary>
/// Move right at 3 meters per second
/// Detect player and deal damage (IDamageable Interface)
/// Destroy this after 5 seconds
/// </summary>
public class AcidEffect : MonoBehaviour
{
    [SerializeField] float m_timeToLive = 5f;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private int m_damageAmount = 1;

    private void Start()
    {
        Destroy(gameObject, m_timeToLive);
    }

    private void Update()
    {
        transform.position += transform.right * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.Damage(m_damageAmount);
    }
}
