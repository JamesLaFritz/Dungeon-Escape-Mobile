public interface IDamageable
{
    /// <summary>
    /// The amount of health.
    /// </summary>
    int Health { get; set; }

    /// <summary>
    /// Apply the amount of damage.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    void Damage(int amount);
}
