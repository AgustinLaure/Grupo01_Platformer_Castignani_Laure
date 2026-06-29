using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJump;
    public event Action OnPlayerAttack;
    public event Action OnPlayerPause;
    enum MoveState
    {
        None,
        Walking,
        Running,
    }

    private const float epsilon = 1e-06f;

    private bool isGrounded = false;
    private bool isTouchingWall = false;
    private bool tryJump = false;

    private MoveState moveState = MoveState.None;
    private MoveState prevMoveState = MoveState.None;

    [SerializeField] private float acceleration;
    [SerializeField] private float walkAcceleration;
    [SerializeField] private float runAcceleration;

    [SerializeField] private float jumpImpulse;

    [SerializeField] private Player player;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private GameObject attackArea;

    private PlayerAnimator playerAnimator;
    private Rigidbody2D rb;

    private const ForceMode2D horizontalMoveForce = ForceMode2D.Force;
    private const ForceMode2D jumpMoveForce = ForceMode2D.Impulse;

    private float terminalVelocity = 0f;

    [SerializeField] private float walkTerminalVelocity;
    [SerializeField] private float runTerminalVelocity;

    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 groundCheckSize;

    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private Vector2 wallCheckOffset;

    private float horizontalAxis = 0f;
    private float prevHorizontalAxis = 0f;

    public bool GetIsGrounded { get { return isGrounded; } }
    public bool GetIsFalling { get { return rb.linearVelocityY < 0f; } }
    public bool GetIsMoving { get { return horizontalAxis > epsilon || horizontalAxis < -epsilon; } }
    public bool GetIsWalking { get { return GetIsMoving && !Input.GetButton("Run"); } }
    public bool GetIsRunning { get { return GetIsMoving && Input.GetButton("Run"); } }
    public float GetHorizontalAxis { get { return horizontalAxis; } }

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<PlayerAnimator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerAnimator.OnFinishAttack += HandleOnFinishAttack;
    }
    private void Update()
    {
        if (!player.IsDead)
        {
            horizontalAxis = Input.GetAxisRaw("Horizontal");

            UpdateMoveState();

            isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, transform.rotation.eulerAngles.z, environmentLayer);
            isTouchingWall = Physics2D.OverlapBox(gameObject.transform.position + new Vector3(horizontalAxis * wallCheckOffset.x, wallCheckOffset.y, 0f), wallCheckSize, 0f, environmentLayer);

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

            if (Input.GetButtonDown("Pause"))
            {
                OnPlayerPause?.Invoke();
            }
        }

        Debug.Log(isTouchingWall);
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
        else if (!isTouchingWall)
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
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(horizontalAxis * wallCheckOffset.x, wallCheckOffset.y, 0f), wallCheckSize);
    }
    private void OnDestroy()
    {
        playerAnimator.OnFinishAttack -= HandleOnFinishAttack;
    }
}
