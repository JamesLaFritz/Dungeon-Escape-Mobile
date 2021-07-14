public class MossGiant : Enemy, IDamageable
{
    #region Implementation of IDamageable

    /// <inheritdoc />
    public void Damage(int amount)
    {
        TriggerGotHit();

        // Subtract amount from health
        Health -= amount;

        if (Health < 1)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
