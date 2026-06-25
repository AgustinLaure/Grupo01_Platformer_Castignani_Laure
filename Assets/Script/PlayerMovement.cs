using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Va en fsm

    enum STATE
    {
        None,
        Idle,
        Walk,
        Air,
        Run
    }

    [SerializeField] private STATE state;

    private bool canJump = false;
    private bool tryJump = false;

    [SerializeField] private float acceleration;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private Rigidbody2D rb;
    private const ForceMode2D forceMode = ForceMode2D.Impulse;

    [SerializeField] private float fallTerminalVelocity;
    [SerializeField] private float walkTerminalVelocity;
    [SerializeField] private float runTerminalVelocity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            tryJump = true;
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal") * acceleration, 0f);

        if (tryJump)
        {
            moveDir.y = jumpAcceleration;
            tryJump = false;
        }

        rb.AddForce(moveDir, forceMode);

        ClampVelocity();
    }

    private void ClampVelocity()
    {
        Vector2 clampValue = new Vector2(
            clampValue.x = state == STATE.Run ? clampValue.x = runTerminalVelocity : clampValue.x = walkTerminalVelocity,
            fallTerminalVelocity);

        rb.linearVelocity = new Vector2(
            Mathf.Clamp(rb.linearVelocity.x, -clampValue.x, clampValue.x),
            Mathf.Clamp(rb.linearVelocity.y, -clampValue.y, float.MaxValue));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }

}
