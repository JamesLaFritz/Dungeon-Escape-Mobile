using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    #region Implementation of IDamageable

    /// <inheritdoc />
    public void Damage(int amount)
    {
        TriggerGotHit();

        Health -= amount;
    }

    #endregion
}
