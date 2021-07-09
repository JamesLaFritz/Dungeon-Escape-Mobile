using UnityEngine;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.name}");

        // if collided with a damageable object do damage.
    }
}
