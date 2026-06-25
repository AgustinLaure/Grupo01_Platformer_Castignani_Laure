using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canJump = false;
    private bool tryJump = false;
    private bool isRunning = false;

    [SerializeField] private float acceleration;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask environmentLayer;

    private const ForceMode2D forceMode = ForceMode2D.Impulse;

    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode runKey;

    [SerializeField] private float fallTerminalVelocity;
    [SerializeField] private float walkTerminalVelocity;
    [SerializeField] private float runTerminalVelocity;
    
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 groundCheckSize;

    private void Update()
    {
        canJump = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, transform.rotation.eulerAngles.z, environmentLayer);
        
        if (Input.GetButtonDown("Jump") && canJump)
        {
            tryJump = true;
        }

       isRunning = Input.GetKeyDown(runKey);
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
            clampValue.x = isRunning ? clampValue.x = runTerminalVelocity : clampValue.x = walkTerminalVelocity,
            fallTerminalVelocity);

        rb.linearVelocity = new Vector2(
            Mathf.Clamp(rb.linearVelocity.x, -clampValue.x, clampValue.x),
            Mathf.Clamp(rb.linearVelocity.y, -clampValue.y, float.MaxValue));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
    }
}
