using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAnimationController m_animation;

    // Start is called before the first frame update
    void Start()
    {
        m_animation = GetComponent<PlayerAnimationController>();
        if (m_animation != null && m_animation.enabled)
        {
            m_animation.EquipFlamingSword();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
