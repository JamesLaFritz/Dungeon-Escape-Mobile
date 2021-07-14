using Cinemachine;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int m_health;

    /// <summary>
    /// The amount of health this enemy has. If the amount is set below 1 the enemy will die.
    /// </summary>
    public int Health
    {
        get => m_health;
        set
        {
            m_health = value;

            if (m_health < 1)
            {
                isDead = true;
            }
        }
    }

    [SerializeField] private float m_destroyTime = 5f;

    private bool m_isDead;

    protected bool isDead
    {
        get { return m_isDead; }
        set
        {
            m_isDead = value;
            if (m_hasAnimator)
                m_animator.SetBool(_isDeadParameterName, value);
            if (!value) return;
            Destroy(gameObject, m_destroyTime);
        }
    }

    [SerializeField] protected float speed;
    [SerializeField] protected int gems;

    [SerializeField] protected CinemachinePathBase waypoints;
    protected bool hasWayPoints;

    private int m_currentWaypoint = 1;
    private int m_currentDirection = 1;

    protected Vector3 currentPosition;
    protected Vector3 nextPosition;

    private bool m_directionChanged;

    [SerializeField] private LayerMask m_combatLayerMask = 1 << 10;
    [SerializeField] private float m_combatDistance = 2f;
    [SerializeField] private Vector3 m_combatCheckOffset = Vector3.zero;

    protected bool inCombat;
    private Vector3 m_combatRayStart;
    private Vector3 m_combatRayDirection;
    private float m_combatRayDistance;
    private Vector3 m_combatDirection;

    private Animator m_animator;
    private bool m_hasAnimator;
    private static int _idleParameterName;
    private static int _hitParameterName;
    private static int _inCombatParameterName;
    private static int _isDeadParameterName;

    protected bool IsIdling => m_hasAnimator && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    protected bool GotHit => m_hasAnimator && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Got Hit");
    protected bool IsAttacking => m_hasAnimator && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    protected bool CanMove => !IsIdling && !GotHit && !IsAttacking && !isDead;

    /// <summary>
    /// Get the first animator component found in children.
    /// </summary>
    protected virtual void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        if (m_animator != null)
        {
            m_hasAnimator = true;
            _idleParameterName = Animator.StringToHash("Idle");
            _hitParameterName = Animator.StringToHash("Hit");
            _inCombatParameterName = Animator.StringToHash("InCombat");
            _isDeadParameterName = Animator.StringToHash("IsDead");
        }

        hasWayPoints = waypoints != null;
        currentPosition = nextPosition = transform.position;
        NextPosition();
    }

    /// <summary>
    /// If is Idling exit.
    /// ChangeFaceDirection
    /// Move
    /// TriggerIdle
    /// NextPosition
    /// </summary>
    protected virtual void Update()
    {
        CombatMode();

        if (!CanMove)
            return;

        ChangeFaceDirection();

        Move();

        TriggerIdle();

        NextPosition();
    }

    /// <summary>
    /// Changes the current way point to the next way point
    /// </summary>
    protected void NextPosition()
    {
        if (!hasWayPoints || currentPosition != nextPosition) return;

        // set the next position to the position at the current way point
        nextPosition = waypoints.EvaluatePositionAtUnit(m_currentWaypoint, CinemachinePathBase.PositionUnits.PathUnits);

        // increase the current way point
        m_currentWaypoint += m_currentDirection;

        // if the current way point is outside the bounds
        if (!(m_currentWaypoint < waypoints.MinUnit(CinemachinePathBase.PositionUnits.PathUnits)) &&
            !(m_currentWaypoint > waypoints.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits))) return;

        // change the direction
        m_directionChanged = true;
        m_currentDirection *= -1;

        // set the current way point
        m_currentWaypoint += m_currentDirection * 2;
    }

    /// <summary>
    /// If the current position has reached the next position and there is an animator trigger the Idle animation
    /// </summary>
    protected void TriggerIdle()
    {
        if (!m_hasAnimator || currentPosition != nextPosition) return;

        m_animator.SetTrigger(_idleParameterName);
    }

    protected void TriggerGotHit()
    {
        if (!m_hasAnimator) return;
        m_animator.SetTrigger(_hitParameterName);
    }

    protected void SetAnimatorInCombat()
    {
        if (m_animator)
            m_animator.SetBool(_inCombatParameterName, inCombat);
    }

    /// <summary>
    /// If the Enemy changes direction change the direction that it is facing
    /// </summary>
    protected void ChangeFaceDirection()
    {
        if (!m_directionChanged) return;

        m_directionChanged = false;
        transform.right = new Vector3(-m_currentDirection, 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    protected void CombatMode()
    {
        m_combatRayStart = transform.position + m_combatCheckOffset - (transform.right * m_combatDistance);
        m_combatRayDirection = transform.right;
        m_combatRayDistance = m_combatDistance * 2;
        RaycastHit2D hit =
            Physics2D.Raycast(m_combatRayStart, m_combatRayDirection, m_combatRayDistance, m_combatLayerMask.value);

        // If it hits something...
        if (hit.collider != null)
        {
            inCombat = true;
            m_combatDirection = (hit.transform.position - transform.position);
            if (m_combatDirection.x < 0)
                transform.right = new Vector3(-1, 0, 0);
            else if (m_combatDirection.x > 0)
                transform.right = new Vector3(1, 0, 0);

            if (m_hasAnimator && CanMove)
                m_animator.SetTrigger(_idleParameterName);
            Debug.DrawRay(m_combatRayStart, m_combatRayDirection * m_combatRayDistance, Color.red);
        }
        else
        {
            if (inCombat)
            {
                m_directionChanged = true;
                inCombat = false;
            }

            Debug.DrawRay(m_combatRayStart, m_combatRayDirection * m_combatRayDistance, Color.green);
        }

        SetAnimatorInCombat();
    }

    /// <summary>
    /// Move towards the next waypoint position.
    /// </summary>
    protected virtual void Move()
    {
        // set the current position using move towards
        currentPosition = Vector3.MoveTowards(currentPosition, nextPosition, speed * Time.deltaTime);
        // set the transform position to the current position
        transform.position = currentPosition;
    }
}
