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
    [SerializeField] private GameObject player;

    private readonly string ControllerStateName = "State";

    private FSM fsm;

    private Dictionary<Type, IState> states;

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
    }

    private void Update()
    {
        fsm.Update();
    }

    private bool CurrentAnimationEnded()
    {
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);

        return animatorInfo.normalizedTime >= 1f;
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
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 0);
        }

        public void Update()
        {
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

        public WalkState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 2);
        }

        public void Update()
        {
            if (!playerAnimator.playerController.GetIsMoving
                    && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
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

    private class RunState : IState
    {
        private PlayerAnimator playerAnimator;

        public RunState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 3);
        }

        public void Update()
        {
            if (!playerAnimator.playerController.GetIsMoving
                    && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
                && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(WalkState));
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

    private class FallState : IState
    {
        private PlayerAnimator playerAnimator;

        public FallState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 5);
        }

        public void Update()
        {
            if (!playerAnimator.playerController.GetIsMoving
                    && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
                && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(WalkState));
            }
            else if (playerAnimator.playerController.GetIsRunning
                 && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(RunState));
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


    private class AttackState : IState
    {
        private PlayerAnimator playerAnimator;

        public AttackState(PlayerAnimator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
        }
        public void Enter()
        {
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 1);
        }

        public void Update()
        {
            if (playerAnimator.CurrentAnimationEnded())
            {
                if (!playerAnimator.playerController.GetIsMoving
                     && playerAnimator.playerController.GetIsGrounded)
                {
                    playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
                }
                else if (playerAnimator.playerController.GetIsWalking
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
        }

        public void Exit()
        {

        }

        public void OnDie()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(DeadState));
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
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 4);
        }

        public void Update()
        {
            if (!playerAnimator.playerController.GetIsMoving
                     && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
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
        public void OnJump()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(JumpState));
        }
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(DeadState));
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
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 6);
        }

        public void Update()
        {
            if (!playerAnimator.playerController.GetIsMoving
                    && playerAnimator.playerController.GetIsGrounded)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(IdleState));
            }
            else if (playerAnimator.playerController.GetIsWalking
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
        public void OnDie()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(DeadState));
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
            playerAnimator.animator.SetFloat(playerAnimator.ControllerStateName, 7);
        }

        public void Update()
        {

        }

        public void Exit()
        {

        }
    }
}
