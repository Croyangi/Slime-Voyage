using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_AirborneState : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {

        if (_helper._movementVars.rawInputMovement == Vector2.zero && _helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper._movementVars.rawInputMovement != Vector2.zero && _helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Moving, out State state))
            {
                TransitionToState(state);
            }
        }

        if (!_helper.isGrounded && _helper.stickingDirection != Vector2.zero && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.rb.velocity.y > 3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING);
        } else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING);
        } else
        {
            //_animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);
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
