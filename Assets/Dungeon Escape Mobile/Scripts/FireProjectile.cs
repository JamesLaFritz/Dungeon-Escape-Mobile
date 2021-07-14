using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private GameObject m_projectile;
    private bool m_hasProjectile;

    [SerializeField] private Transform m_firePoint;
    private Vector3 m_firePosition;

    private void Start()
    {
        m_hasProjectile = m_projectile != null;
        m_firePosition = m_firePoint != null ? m_firePoint.position : transform.position;
    }

    public void Fire()
    {
        if (!m_hasProjectile) return;

        Instantiate(m_projectile, m_firePosition, Quaternion.identity);
    }
}
