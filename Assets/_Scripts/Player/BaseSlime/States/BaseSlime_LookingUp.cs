using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_LookingUp : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        if ((!_helper.isGrounded || _helper._movementVars.processedInputMovement.y < 1f) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        _animator.ChangeAnimationState(_animator.BASESLIME_LOOKINGUP);
    }


    public override void ExitState()
    {
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
