using System.Collections.Generic;
using System;
using UnityEngine;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject player;

    private readonly string controllerStateName = "State";
    private int controllerStateHash;

    [SerializeField] private float stillTimeToIdleFromMove = 0.25f;

    private FSM fsm;

    private Dictionary<Type, IState> states;

    private float horizontalAxis = 0f;
    private float prevHorizontalAxis = 0f;

    private void Awake()
    {
        controllerStateHash = Animator.StringToHash(controllerStateName);
    }
    private void Start()
    {
        IdleState idleState = new IdleState(this);
        playerController.OnPlayerAttack += idleState.OnAttack;
        playerController.OnPlayerJump += idleState.OnJump;

        WalkState walkState = new WalkState(this);
        playerController.OnPlayerAttack += walkState.OnAttack;
        playerController.OnPlayerJump += walkState.OnJump;
        //OnHurt
        //Ondie

        RunState runState = new RunState(this);
        playerController.OnPlayerAttack += runState.OnAttack;
        playerController.OnPlayerJump += runState.OnJump;
        //OnHurt
        //Ondie

        FallState fallState = new FallState(this);
        playerController.OnPlayerAttack += fallState.OnAttack;
        playerController.OnPlayerJump += fallState.OnJump;
        //OnHurt
        //OnDie

        AttackState attackState = new AttackState(this);
        //Ondie

        HurtState hurtState = new HurtState(this);
        playerController.OnPlayerAttack += hurtState.OnAttack;
        playerController.OnPlayerJump += hurtState.OnJump;
        //Ondie

        JumpState jumpState = new JumpState(this);
        playerController.OnPlayerAttack += jumpState.OnAttack;
        //OnHurt
        //Ondie

        DeadState deadState = new DeadState(this);

        states = new Dictionary<Type, IState>()
        {
            [typeof(IdleState)] = idleState,
            [typeof(AttackState)] = attackState,
            [typeof(WalkState)] = walkState,
            [typeof(RunState)] = runState,
            [typeof(FallState)] = fallState,
            [typeof(HurtState)] = hurtState,
            [typeof(JumpState)] = jumpState,
            [typeof(DeadState)] = deadState
        };

        fsm = new FSM(states);
        fsm.SetInitialState(typeof(IdleState));

        prevHorizontalAxis = playerController.GetHorizontalAxis;
    }

    private void Update()
    {
        horizontalAxis = playerController.GetHorizontalAxis;

        fsm.Update();

        Debug.Log("Curr: " + playerController.GetHorizontalAxis);
        Debug.Log("Prev: " + prevHorizontalAxis);

        prevHorizontalAxis = horizontalAxis;
    }

    private bool CurrentAnimationEnded()
    {
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);

        return animatorInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }

    private void OnDestroy()
    {
        IState auxState;

        states.TryGetValue(typeof(IdleState), out auxState);
        playerController.OnPlayerAttack -= ((IdleState)auxState).OnAttack;
        playerController.OnPlayerJump -= ((IdleState)auxState).OnJump;

        states.TryGetValue(typeof(WalkState), out auxState);
        playerController.OnPlayerAttack -= ((WalkState)auxState).OnAttack;
        playerController.OnPlayerJump -= ((WalkState)auxState).OnJump;

        states.TryGetValue(typeof(RunState), out auxState);
        playerController.OnPlayerAttack -= ((RunState)auxState).OnAttack;
        playerController.OnPlayerJump -= ((RunState)auxState).OnJump;

        states.TryGetValue(typeof(FallState), out auxState);
        playerController.OnPlayerAttack -= ((FallState)auxState).OnAttack;
        playerController.OnPlayerJump -= ((FallState)auxState).OnJump;

        states.TryGetValue(typeof(HurtState), out auxState);
        playerController.OnPlayerAttack -= ((HurtState)auxState).OnAttack;
        playerController.OnPlayerJump -= ((HurtState)auxState).OnJump;

        states.TryGetValue(typeof(JumpState), out auxState);
        playerController.OnPlayerAttack -= ((JumpState)auxState).OnAttack;
    }

    private class IdleState : IState
    {
        private PlayerAnimator playerAnimator;

        public IdleState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 0);
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (playerAnimator.playerController.GetIsWalking
                && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(WalkState));
            }
            else if (playerAnimator.playerController.GetIsRunning
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(RunState));
            }
            else if (playerAnimator.playerController.GetIsFalling)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(AttackState));
        }

        public void OnHurt()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(HurtState));
        }
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(JumpState));
        }

        public void OnDie()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(DeadState));
        }
    }

    private class WalkState : IState
    {
        private PlayerAnimator playerAnimator;
        private float timeStill = 0f;


        public WalkState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 1);
            timeStill = 0f;
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (!playerAnimator.playerController.GetIsMoving
                && playerAnimator.playerController.GetIsGrounded)
            {
                timeStill += Time.deltaTime;
            }
            else
            {
                timeStill = 0f;
            }

            if (timeStill >= playerAnimator.stillTimeToIdleFromMove)
            {
                playerAnimator.fsm.TryChange<WalkState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsRunning
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<WalkState>(typeof(RunState));
            }
            else if (playerAnimator.playerController.GetIsFalling)
            {
                playerAnimator.fsm.TryChange<WalkState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<WalkState>(typeof(AttackState));
        }
        public void OnHurt()
        {
            playerAnimator.fsm.TryChange<WalkState>(typeof(HurtState));
        }
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<WalkState>(typeof(JumpState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<WalkState>(typeof(DeadState));
        }
    }

    private class RunState : IState
    {
        private PlayerAnimator playerAnimator;
        private float timeStill = 0f;
        public RunState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 2);
            timeStill = 0f;
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (!playerAnimator.playerController.GetIsMoving
                && playerAnimator.playerController.GetIsGrounded)
            {
                timeStill += Time.deltaTime;
            }
            else
            {
                timeStill = 0f;
            }

            if (timeStill >= playerAnimator.stillTimeToIdleFromMove)
            {
                playerAnimator.fsm.TryChange<RunState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
                && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<RunState>(typeof(WalkState));
            }
            else if (playerAnimator.playerController.GetIsFalling)
            {
                playerAnimator.fsm.TryChange<RunState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<RunState>(typeof(AttackState));
        }
        public void OnHurt()
        {
            playerAnimator.fsm.TryChange<RunState>(typeof(HurtState));
        }
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<RunState>(typeof(JumpState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<RunState>(typeof(DeadState));
        }
    }

    private class FallState : IState
    {
        private PlayerAnimator playerAnimator;

        public FallState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 3);
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (!playerAnimator.playerController.GetIsMoving
                    && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<FallState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
                && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<FallState>(typeof(WalkState));
            }
            else if (playerAnimator.playerController.GetIsRunning
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<FallState>(typeof(RunState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<FallState>(typeof(AttackState));
        }
        public void OnHurt()
        {
            playerAnimator.fsm.TryChange<FallState>(typeof(HurtState));
        }
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<FallState>(typeof(JumpState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<FallState>(typeof(DeadState));
        }
    }

    private class AttackState : IState
    {
        private PlayerAnimator playerAnimator;

        public AttackState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 4);
        }

        public void Update()
        {
            if (playerAnimator.CurrentAnimationEnded())
            {
                if (playerAnimator.horizontalAxis > 0)
                {
                    playerAnimator.spriteRenderer.flipX = false;
                }
                else if (playerAnimator.horizontalAxis < 0)
                {
                    playerAnimator.spriteRenderer.flipX = true;
                }

                if (!playerAnimator.playerController.GetIsMoving
                     && playerAnimator.playerController.GetIsGrounded)
                {
                    playerAnimator.fsm.TryChange<AttackState>(typeof(IdleState));
                }
                else if (playerAnimator.playerController.GetIsWalking
                     && playerAnimator.playerController.GetIsGrounded)
                {
                    playerAnimator.fsm.TryChange<AttackState>(typeof(WalkState));
                }
                else if (playerAnimator.playerController.GetIsRunning
                     && playerAnimator.playerController.GetIsGrounded)
                {
                    playerAnimator.fsm.TryChange<AttackState>(typeof(RunState));
                }
                else if (playerAnimator.playerController.GetIsFalling)
                {
                    playerAnimator.fsm.TryChange<AttackState>(typeof(FallState));
                }
            }
        }

        public void Exit()
        {

        }

        public void OnDie()
        {
            playerAnimator.fsm.TryChange<AttackState>(typeof(DeadState));
        }
    }

    private class HurtState : IState
    {
        private PlayerAnimator playerAnimator;

        public HurtState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 5);
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (!playerAnimator.playerController.GetIsMoving
                     && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<HurtState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<HurtState>(typeof(WalkState));
            }
            else if (playerAnimator.playerController.GetIsRunning
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<HurtState>(typeof(RunState));
            }
            else if (playerAnimator.playerController.GetIsFalling)
            {
                playerAnimator.fsm.TryChange<HurtState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<HurtState>(typeof(AttackState));
        }
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<HurtState>(typeof(JumpState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<HurtState>(typeof(DeadState));
        }
    }

    private class JumpState : IState
    {
        private PlayerAnimator playerAnimator;

        public JumpState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 6);
        }

        public void Update()
        {
            if (playerAnimator.horizontalAxis > 0)
            {
                playerAnimator.spriteRenderer.flipX = false;
            }
            else if (playerAnimator.horizontalAxis < 0)
            {
                playerAnimator.spriteRenderer.flipX = true;
            }

            if (playerAnimator.playerController.GetIsFalling)
            {
                playerAnimator.fsm.TryChange<JumpState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        public void OnAttack()
        {
            playerAnimator.fsm.TryChange<JumpState>(typeof(AttackState));
        }
        public void OnHurt()
        {
            playerAnimator.fsm.TryChange<JumpState>(typeof(HurtState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<JumpState>(typeof(DeadState));
        }
    }

    private class DeadState : IState
    {
        private PlayerAnimator playerAnimator;

        public DeadState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetInteger(playerAnimator.controllerStateHash, 7);
        }

        public void Update()
        {

        }

        public void Exit()
        {

        }
    }
}
