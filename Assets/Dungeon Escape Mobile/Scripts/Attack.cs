using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make sure the Attack Animation enables & disables this Behavior when Attacking.
/// </summary>
public class Attack : MonoBehaviour
{
    private List<IDamageable> hits;

    private void OnEnable()
    {
        hits = new List<IDamageable>();
    }

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
    }
}
