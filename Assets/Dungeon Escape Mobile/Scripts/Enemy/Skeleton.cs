using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    #region Implementation of IDamageable

    /// <inheritdoc />
    public void Damage(int amount)
    {
        Debug.Log($"{name} has been damaged by {amount}");

        // Subtract amount from health
        Health -= amount;

        if (Health < 1)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
