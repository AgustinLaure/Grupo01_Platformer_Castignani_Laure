using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJump;
    public event Action OnPlayerAttack;
    enum MoveState
    {
        None,
        Walking,
        Running,
    }

    private const float epsilon = 1e-06f;

    private bool isGrounded = false;
    private bool tryJump = false;

    private MoveState moveState = MoveState.None;
    private MoveState prevMoveState = MoveState.None;

    [SerializeField] private float acceleration;
    [SerializeField] private float walkAcceleration;
    [SerializeField] private float runAcceleration;

    [SerializeField] private float jumpImpulse;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private PlayerAnimator playerAnimator;

    private const ForceMode2D horizontalMoveForce = ForceMode2D.Force;
    private const ForceMode2D jumpMoveForce = ForceMode2D.Impulse;

    private float terminalVelocity = 0f;

    [SerializeField] private float walkTerminalVelocity;
    [SerializeField] private float runTerminalVelocity;

    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 groundCheckSize;

    private float horizontalAxis = 0f;
    private float prevHorizontalAxis = 0f;

    public bool GetIsGrounded { get { return isGrounded; } }
    public bool GetIsFalling { get { return rb.linearVelocityY < 0f; } }
    public bool GetIsMoving { get { return horizontalAxis > epsilon || horizontalAxis < -epsilon; } }
    public bool GetIsWalking { get { return GetIsMoving && !Input.GetButton("Run"); } }
    public bool GetIsRunning { get { return GetIsMoving && Input.GetButton("Run"); } }
    public float GetHorizontalAxis { get { return horizontalAxis; } }

    private void Start()
    {
        playerAnimator.OnFinishAttack += HandleOnFinishAttack;
    }
    private void Update()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");

        UpdateMoveState();

        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, transform.rotation.eulerAngles.z, environmentLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            tryJump = true;
            OnPlayerJump?.Invoke();
        }

        if (Input.GetButtonDown("Attack"))
        {
            attackArea.gameObject.SetActive(true);

            OnPlayerAttack?.Invoke();
        }
    }

    private void OnDestroy()
    {
        playerAnimator.OnFinishAttack -= HandleOnFinishAttack;
    }

    private void UpdateMoveState()
    {
        switch (moveState)
        {
            case MoveState.None:

                if (GetIsMoving)
                {
                    if (GetIsRunning)
                    {
                        moveState = MoveState.Running;
                    }
                    else if (GetIsWalking)
                    {
                        moveState = MoveState.Walking;
                    }
                }

                break;

            case MoveState.Walking:

                if (moveState != prevMoveState)
                {
                    prevMoveState = moveState;

                    moveState = MoveState.Walking;
                    terminalVelocity = walkTerminalVelocity;
                    acceleration = walkAcceleration;
                }

                if (!GetIsMoving)
                {
                    moveState = MoveState.None;
                }
                else if (GetIsRunning)
                {
                    moveState = MoveState.Running;
                }

                break;

            case MoveState.Running:

                if (moveState != prevMoveState)
                {
                    prevMoveState = moveState;

                    moveState = MoveState.Running;
                    terminalVelocity = runTerminalVelocity;
                    acceleration = runAcceleration;
                }

                if (!GetIsMoving)
                {
                    moveState = MoveState.None;
                }
                else if (GetIsWalking)
                {
                    moveState = MoveState.Walking;
                }

                break;

            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontalAxis) < epsilon &&
            Mathf.Abs(horizontalAxis - prevHorizontalAxis) > epsilon)
        {
            AddStopForce();
        }
        else
        {
            float horizontalMove = horizontalAxis * acceleration;

            float velocityDist = terminalVelocity - rb.linearVelocityX * Mathf.Sign(horizontalMove);

            float maxPossibleAccel = (velocityDist * rb.mass) / Time.fixedDeltaTime;

            horizontalMove = Mathf.Clamp(Mathf.Abs(horizontalMove), 0f, maxPossibleAccel) * Mathf.Sign(horizontalMove);

            rb.AddForce(new Vector2(horizontalMove, 0f), horizontalMoveForce);
        }

        if (tryJump)
        {
            rb.AddForce(new Vector2(0f, jumpImpulse), jumpMoveForce);
            tryJump = false;
        }

        prevHorizontalAxis = horizontalAxis;
    }

    private void AddStopForce()
    {
        float velocityClamp = 0f;

        velocityClamp = moveState == MoveState.Running ? runTerminalVelocity : walkTerminalVelocity;

        velocityClamp = Mathf.Min(Mathf.Abs(rb.linearVelocityX), velocityClamp);

        rb.linearVelocityX -= velocityClamp * Mathf.Sign(rb.linearVelocityX);
    }

    private void HandleOnFinishAttack()
    {
        attackArea.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
    }
}
