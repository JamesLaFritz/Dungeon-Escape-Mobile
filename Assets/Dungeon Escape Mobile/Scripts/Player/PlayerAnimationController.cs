using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    #region Speed

    private int m_speedParameter;
    private bool m_hasSpeedParameter;

    public void SetAnimatorMoveSpeedParameter(float value)
    {
        if (!m_hasSpeedParameter) return;
        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetFloat(m_speedParameter, value);
    }

    #endregion

    #region Jump

    private int m_jumpParameter;
    private bool m_hasJumpParameter;

    public void TriggerJump()
    {
        if (!m_hasJumpParameter) return;

        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetTrigger(m_jumpParameter);
    }

    #endregion

    #region Attack

    private int m_attackParameter;
    private bool m_hasAttackParameter;

    [SerializeField] private Animator m_swordAnim;

    public void TriggerAttack()
    {
        if (!m_hasAttackParameter) return;

        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetTrigger(m_attackParameter);

        if (m_swordAnim != null)
        {
            m_swordAnim.SetTrigger("Attack");
        }
    }

    #endregion

    #region Got Hit

    private int m_hitParameter;
    private bool m_hasHitParameter;

    public void TriggerGotHit()
    {
        if (!m_hasHitParameter) return;

        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetTrigger(m_hitParameter);
    }

    #endregion

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        if (m_animator == null)
        {
            enabled = false;
        }
    }

    private void Start()
    {
        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");

        if (m_animator.GetParameter(GetAnimatorParameterIndex("Speed")) != null)
        {
            m_hasSpeedParameter = true;
            m_speedParameter = Animator.StringToHash("Speed");
        }

        if (m_animator.GetParameter(GetAnimatorParameterIndex("Jump")) != null)
        {
            m_hasJumpParameter = true;
            m_jumpParameter = Animator.StringToHash("Jump");
        }

        if (m_animator.GetParameter(GetAnimatorParameterIndex("Attack")) != null)
        {
            m_hasAttackParameter = true;
            m_attackParameter = Animator.StringToHash("Attack");
        }

        if (m_animator.GetParameter(GetAnimatorParameterIndex("GotHit")) != null)
        {
            m_hasHitParameter = true;
            m_hitParameter = Animator.StringToHash("GotHit");
        }
    }

    public byte GetAnimatorParameterIndex(string paramName)
    {
        for (byte i = 0; i < m_animator.parameters.Length; i++)
        {
            if (m_animator.parameters[i].name == paramName)
            {
                return i;
            }
        }

        Debug.LogError("Parameter " + paramName + " doesn't exist in the animator parameter list!");
        return 0;
    }

    public void ActivateLayer(int layerIndex)
    {
        m_animator.SetLayerWeight(layerIndex, 1);
    }

    public void DeactivateLayer(int layerIndex)
    {
        m_animator.SetLayerWeight(layerIndex, 0);
    }

    public void EquipFlamingSword()
    {
        ActivateLayer(m_animator.GetLayerIndex("Flame Sword"));
    }

    public void UnEquipFlamingSword()
    {
        DeactivateLayer(m_animator.GetLayerIndex("Flame Sword"));
    }
}
