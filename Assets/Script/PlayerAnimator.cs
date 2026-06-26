using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack,
        Walk,
        Run,
        Hurt,
        Fall,
        Jump,
        Death
    }

    [SerializeField] PlayerController playerController;

    private State currentState = State.Idle;
    private State prevState = State.Idle;
    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:

                break;

            case State.Attack:

                break;

            case State.Walk:

                break;

            case State.Run:

                break;

            case State.Hurt:

                break;

            case State.Fall:

                break;

            case State.Jump:

                break;

            case State.Death:

                break;

            default:
                break;
        }
    }
}
