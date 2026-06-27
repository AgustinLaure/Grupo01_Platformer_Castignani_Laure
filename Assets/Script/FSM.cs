using System;
using System.Collections.Generic;

public class FSM
{
    private IState currentState;

    private Dictionary<Type, IState> statesDictionary = new Dictionary<Type, IState>();

    public FSM(List<IState> states)
    {
        foreach (IState state in states)
        {
            statesDictionary.Add(state.GetType(), state);
        }
    }

    public void Update()
    {
        currentState.Update();
    }

    public void TryChange<T>(Type toState) where T : IState
    {
        if (currentState is T)
        {
            currentState?.Exit();
            statesDictionary.TryGetValue(toState, out currentState);
            currentState?.Enter();
        }
    }
}
