using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private Vector2 m_look;
    private Vector2 m_move;

    private bool m_FireButtonIsBeingPressed;
    private bool m_FireActionPerformed;

    // 'Fire' input action has been triggered.
    public void OnFire(InputAction.CallbackContext context)
    {
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
                break;
        }
    }

    // 'Move' input action has been performed.
    public void OnMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
    }

    // 'Look' input action has been performed.
    public void OnLook(InputAction.CallbackContext context)
    {
        m_look = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        // Update transform from m_Move and m_Look
        Fire();
    }

    private void Fire()
    {
        if (m_FireActionPerformed)
        {
            // special fire action performed code
            m_FireActionPerformed = false;
        }
    }
}
