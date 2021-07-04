using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private Vector2 m_move;

    private bool m_FireButtonIsBeingPressed;
    private bool m_FireActionPerformed;

    private PlayerInput m_playerInput;
    private bool m_playerInputHasBeenInit;

    private InputAction m_moveAction;

    // 'Fire' input action has been triggered.
    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log($"Fire Event {context.phase}");
        switch (context.phase)
        {
            // The Button was pressed
            case InputActionPhase.Started:
                m_FireButtonIsBeingPressed = true;
                break;
            // The Action has meet all preformed Conditions
            case InputActionPhase.Performed:
                m_FireActionPerformed = true;
                break;
            // The Button was released
            case InputActionPhase.Canceled:
                m_FireButtonIsBeingPressed = false;
                m_FireActionPerformed = false;
                break;
            case InputActionPhase.Disabled:
            case InputActionPhase.Waiting:
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_moveAction = m_playerInput.actions["Move"];
    }

    private void OnDisable()
    {
        m_playerInput.actions["Fire"].performed -= OnFire;
        m_playerInput.actions["Fire"].started -= OnFire;
        m_playerInput.actions["Fire"].canceled -= OnFire;

        m_playerInputHasBeenInit = false;
    }

    private void InitPlayerInput()
    {
        if (!m_playerInput.isActiveAndEnabled) return;

        m_playerInputHasBeenInit = true;

        m_playerInput.actions["Fire"].performed += OnFire;
        m_playerInput.actions["Fire"].started += OnFire;
        m_playerInput.actions["Fire"].canceled += OnFire;
    }

    public void Update()
    {
        if (!m_playerInputHasBeenInit)
            InitPlayerInput();

        m_move = m_moveAction.ReadValue<Vector2>();

        // Update transform from m_Move and m_Look
    }
}
