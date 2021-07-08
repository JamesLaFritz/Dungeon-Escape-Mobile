using System.Collections;
using Cinemachine;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int gems;

    [SerializeField] protected float idleWaitTime;

    [SerializeField] protected CinemachinePathBase waypoints;
    protected bool hasWayPoints;
    private int m_currentWaypoint = 1;
    private int m_currentDirection = 1;
    protected Vector3 currentPosition;
    protected Vector3 nextPosition;
    private bool m_directionChanged;

    private Animator m_animator;
    private bool m_hasAnimator;
    private static int _idleParameterName;

    protected virtual void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        if (m_animator != null)
        {
            m_hasAnimator = true;
            _idleParameterName = Animator.StringToHash("Idle");
        }

        hasWayPoints = waypoints != null;
        currentPosition = nextPosition = transform.position;
        NextPosition();
    }

    protected virtual void Update()
    {
        if (m_hasAnimator && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        if (m_directionChanged)
        {
            m_directionChanged = false;
            transform.right = new Vector3(-m_currentDirection, 0, 0);
        }

        Move();
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
    /// While the current position is not the next position, move towards the next position.
    /// Once the enemy reaches the next position get the next position to move to and set the State to Idle
    /// </summary>
    protected virtual void Move()
    {
        // set the current position using move towards
        currentPosition = Vector3.MoveTowards(currentPosition, nextPosition, speed * Time.deltaTime);
        // set the transform position to the current position
        transform.position = currentPosition;

        if (currentPosition == nextPosition && m_hasAnimator)
            m_animator.SetTrigger(_idleParameterName);

        // Get the Next Position
        NextPosition();
    }
}
