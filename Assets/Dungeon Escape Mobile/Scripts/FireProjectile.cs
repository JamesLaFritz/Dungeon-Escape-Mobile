using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private GameObject m_projectile;
    private bool m_hasProjectile;

    [SerializeField] private Transform m_firePoint;
    private Vector3 m_firePosition;
    private Quaternion m_fireRotation;

    private void Start()
    {
        m_hasProjectile = m_projectile != null;
        bool hasFirePoint = m_firePoint != null;
        m_firePosition = hasFirePoint ? m_firePoint.position : transform.position;
        m_fireRotation = hasFirePoint ? m_firePoint.rotation : transform.rotation;
    }

    public void Fire()
    {
        if (!m_hasProjectile) return;

        Instantiate(m_projectile, m_firePosition, m_fireRotation);
    }
}
