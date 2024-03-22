using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_StickingState : State
{
    public enum PlayerStates
    {
        Moving, Jumping, Airborne, OnEdge, Compressed, LookingUp
    }

    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        //_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && _rigidBody2D.velocity.y < 0.1 && _stateHandler.isOnEdge == 0
        if ( _helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }

        if (!_helper.isGrounded && _helper.stickingDirection == Vector2.zero && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.stickingDirection.x == 1)
        {
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _animator.FlipSprite(false);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        _animator.ChangeAnimationState(_animator.BASESLIME_STICK);
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
