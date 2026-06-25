using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Va en fsm

    enum STATE
    {
        None,
        Idle,
        Walk,
        Run
    }

    [SerializeField] private STATE state;

    [SerializeField] private float acceleration;
    [SerializeField] private Rigidbody2D rb;
    private const ForceMode2D forceMode = ForceMode2D.Impulse;

    [SerializeField] private float fallTerminalVelocity;
    [SerializeField] private float walkTerminalVelocity;
    [SerializeField] private float runTerminalVelocity;

    private void FixedUpdate()
    {
        Vector2 nextDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.AddForce(nextDir * acceleration, forceMode);

        ClampVelocity();
    }

    private void ClampVelocity()
    {
        Vector2 clampValue = new Vector2(
            clampValue.x = state == STATE.Run ? clampValue.x = runTerminalVelocity : clampValue.x = walkTerminalVelocity,
            fallTerminalVelocity);

        rb.linearVelocity = new Vector2(
            Mathf.Clamp(rb.linearVelocity.x, -clampValue.x, clampValue.x),
            Mathf.Clamp(rb.linearVelocity.y, -clampValue.y, clampValue.y));
    }
}
