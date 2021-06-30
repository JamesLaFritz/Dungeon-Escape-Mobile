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

    public void PlayIdleAnimation() => SetAnimatorMoveSpeedParameter(0);

    public void PlayWalkAnimation() => SetAnimatorMoveSpeedParameter(1);

    public void PlayRunAnimation() => SetAnimatorMoveSpeedParameter(2);

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
