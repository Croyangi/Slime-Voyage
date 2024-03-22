using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_IdleState : State
{
    public enum PlayerStates
    {
        Moving, Jumping, Airborne, Compressed, LookingUp
    }

    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        //_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && _rigidBody2D.velocity.y < 0.1 && _stateHandler.isOnEdge == 0
        if (_helper._movementVars.processedInputMovement.x != 0 && _helper.isGrounded && Mathf.Abs(_helper.rb.velocity.x) > 0.1f && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Moving, out State state))
            {
                TransitionToState(state);
            }
        }

        if (!_helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.isGrounded && _helper._movementVars.processedInputMovement.x == 0 && _helper._movementVars.processedInputMovement.y == -1 && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Compressed, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.isGrounded && _helper._movementVars.processedInputMovement.x == 0 && _helper._movementVars.processedInputMovement.y == 1 && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.LookingUp, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.isOnEdge != 0)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_ONEDGE);

            if (_helper.isOnEdge == 1)
            {
                _animator.FlipSprite(true);
            } else
            {
                _animator.FlipSprite(false);
            }
        } else
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_IDLE);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        _animator.ChangeAnimationState(_animator.BASESLIME_IDLE);
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
