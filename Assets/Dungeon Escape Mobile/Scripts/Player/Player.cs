using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_health = 3;

    private bool m_isDead;

    public bool IsDead
    {
        get => m_isDead;
        set
        {
            m_isDead = value;
            if (m_hasPlayerAnimationController)
                m_playerAnimationController.IsDead(value);
            if (m_isDead) Die();
        }
    }

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
        if (m_isDead) return;

        m_velocity = m_rigidbody2D.velocity;

        Move();
        Jump();
        Attack();

        m_rigidbody2D.velocity = m_velocity;

        //Debug.DrawRay(transform.position, Vector3.down * m_groundCheckDistance, Color.green);
        //Debug.DrawRay(transform.position, Vector3.right * m_groundCheckDistance, Color.yellow);
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
        if (!m_jumpAction.triggered || !IsGrounded()) return;

        m_velocity.y = m_jumpVelocity;

        if (m_hasPlayerAnimationController)
            m_playerAnimationController.TriggerJump();
    }

    private void Attack()
    {
        if (!m_fireAction.triggered) return;

        if (!IsGrounded()) return;

        if (m_hasPlayerAnimationController)
            m_playerAnimationController.TriggerAttack();
    }

    private void TriggerGotHit()
    {
        if (m_hasPlayerAnimationController)
            m_playerAnimationController.TriggerGotHit();
    }

    private void Die()
    {
        
    }

    #region Implementation of IDamageable

    /// <inheritdoc />
    public int Health
    {
        get => m_health;
        set
        {
            m_health = value;
            if (m_health < 1 && !m_isDead)
            {
                IsDead = true;
            }
        }
    }

    /// <inheritdoc />
    public void Damage(int amount)
    {
        if (m_isDead) return;
        TriggerGotHit();

        // Subtract amount from health
        Health -= amount;
    }

    #endregion
}
