using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float damageableResetTime = 0.2f;
    private List<IDamageable> hits = new List<IDamageable>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if collided with a damageable object do damage.
        IDamageable damageable = other.GetComponent<IDamageable>();
        // If Damageable is null there is nothing to damage exit
        if (damageable == null)
        {
            Debug.Log($"{other.name} is not a damageable object.");
            return;
        }

        if (hits.Contains(damageable)) return;

        hits.Add(damageable);
        damageable.Damage(1);
        StartCoroutine(RemoveDamageable(damageable));
    }

    private IEnumerator RemoveDamageable(IDamageable damageable)
    {
        yield return new WaitForSeconds(damageableResetTime);

        hits.Remove(damageable);
    }
}
