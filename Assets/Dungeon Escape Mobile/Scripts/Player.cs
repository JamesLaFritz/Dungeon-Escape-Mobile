using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float m_walkSpeed = 1f;

    [SerializeField] private float m_jumpVelocity = 5f;
    private bool m_isJumping;

    private float m_moveSpeed;
    private Vector2 m_move;
    private Vector2 m_velocity = Vector2.zero;

    [Header("Ground Check")]
    [SerializeField]
    private LayerMask m_groundLayer;

    [SerializeField] private float m_groundCheckDistance = 0.6f;

    private Rigidbody2D m_rigidbody2D;

    private PlayerInput m_playerInput;

    private InputAction m_moveAction;
    private InputAction m_fireAction;
    private InputAction m_jumpAction;

    private PlayerAnimationController m_playerAnimationController;
    private bool m_hasPlayerAnimationController;

    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_moveAction = m_playerInput.actions["Move"];
        m_fireAction = m_playerInput.actions["Fire"];
        m_jumpAction = m_playerInput.actions["Jump"];

        m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_playerAnimationController = GetComponent<PlayerAnimationController>();
        m_hasPlayerAnimationController = m_playerAnimationController != null;
    }

    public void Update()
    {
        m_velocity = m_rigidbody2D.velocity;

        Move();
        Jump();

        m_rigidbody2D.velocity = m_velocity;
    }

    private void Move()
    {
        m_move.x = m_moveAction.ReadValue<Vector2>().x;
        if (m_move.x > 0.1f || m_move.x < -0.1f)
        {
            m_moveSpeed = m_walkSpeed;
            transform.right = m_move;
        }
        else
        {
            m_moveSpeed = 0;
        }

        m_velocity.x = m_move.x * m_moveSpeed;

        if (m_hasPlayerAnimationController)
            m_playerAnimationController.SetAnimatorMoveSpeedParameter(m_moveSpeed);
    }

    private bool IsGrounded()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, m_groundCheckDistance, m_groundLayer.value);

        // If it hits something...
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        if (!m_jumpAction.triggered) return;

        if (IsGrounded())
        {
            m_velocity.y = m_jumpVelocity;
            Debug.Log("Jump");

            if (m_hasPlayerAnimationController)
                m_playerAnimationController.TriggerJump();
        }
    }
}
