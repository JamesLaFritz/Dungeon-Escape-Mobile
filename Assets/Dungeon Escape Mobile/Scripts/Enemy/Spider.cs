using UnityEngine;

public class Spider : Enemy, IDamageable
{
    [SerializeField] private float m_faceDirection = -1;
    
    #region Implementation of IDamageable

    /// <inheritdoc />
    public void Damage(int amount)
    {
        Health = Health - amount;
    }

    #endregion

    private void Awake()
    {
        transform.right = new Vector3(m_faceDirection, 0, 0);
    }

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
