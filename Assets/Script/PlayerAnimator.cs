using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private readonly string ControllerStateName = "State";

    private FSM stateMachine;

    private Dictionary<Type, IState> states = new Dictionary<Type, IState>()
    {
        [typeof(WalkState)] = new WalkState()
    };

    private void Start()
    {
       

        toAddStates[]

        stateMachine = new FSM(toAddStates);
    }

    private void Update()
    {
        stateMachine.Update();
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
            if (true)
            {

            }
        }

        public void Exit()
        {

        }

        private void OnDeath()
        {

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
}
