using UnityEngine;

public class Attack : MonoBehaviour
{
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

        damageable.Damage(1);
    }
}
