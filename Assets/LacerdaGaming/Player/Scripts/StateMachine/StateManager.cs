using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    public readonly Dictionary<EState, BaseState<EState>> States = new();
    protected BaseState<EState> CurrentState;

    protected bool isTransitioningState = false;

    void Start()
    {
        CurrentState.EnterState();
    }

    public virtual void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState stateKey)
    {
        isTransitioningState = true;

        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();

        isTransitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}
