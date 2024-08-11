using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public State StateKey { get; private set; }

    public void ModifyStateKey(State state)
    {
        StateKey = state;
    }

    public abstract void EnterState();
    public abstract void FixedUpdateState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void TransitionToState(State state);
}
