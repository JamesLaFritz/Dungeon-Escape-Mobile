public class Spider : Enemy, IDamageable
{
    #region Implementation of IDamageable

    /// <inheritdoc />
    public void Damage(int amount) { }

    #endregion

    #region Overrides of Enemy

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();
        inCombat = true;
        SetAnimatorInCombat();
    }

    /// <inheritdoc />
    protected override void Update()
    {
        if (!CanMove)
            return;

        TriggerIdle();
    }

    #endregion
}
