using System.Collections.Generic;
using System;
using UnityEngine;
using NUnit.Framework.Interfaces;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private readonly string ControllerStateName = "State";

    private FSM fsm;

    private Dictionary<Type, IState> states = new Dictionary<Type, IState>()
    {
        //[typeof(WalkState)] = new WalkState()
    };

    private void Start()
    {
       

       // toAddStates[]

        //stateMachine = new FSM(toAddStates);
    }

    private void Update()
    {
        fsm.Update();
    }

    private void OnDestroy()
    {
        
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
            if (playerAnimator.rb2d.linearVelocityY < 0f)
            {
                playerAnimator.fsm.TryChange<IdleState>(typeof(FallState));
            }
        }

        public void Exit()
        {

        }

        private void OnAttack()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(AttackState));
        }

        private void OnWalk()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(WalkState));
        }

        private void OnRun()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(RunState));
        }

        private void OnJump()
        {
            playerAnimator.fsm.TryChange<IdleState>(typeof(JumpState));
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

        }

        public void Exit()
        {

        }

        private void OnDeath()
        {
            
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

        }

        public void Exit()
        {

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

        }

        public void Exit()
        {

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

        }

        public void Exit()
        {

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

        }

        public void Exit()
        {

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

        }

        public void Exit()
        {

        }
    }

    private class DeathState : IState
    {
        private PlayerAnimator playerAnimator;

        public DeathState(PlayerAnimator playerAnimator)
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
