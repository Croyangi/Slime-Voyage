using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_MovingState : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private BaseSlime_Movement _movement;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        if ((_helper._movementVars.processedInputMovement.x == 0 || _helper._movementVars.processedInputMovement.x == _helper.touchingDirection.x) && _helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
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

        if (_helper.isRunning == true)
        {
            _animator.ChangeAnimationState(_animator.EYES_SCARED, _animator.eyes_animator);
        } else {
            _animator.ChangeAnimationState(_animator.EYES_MOVING, _animator.eyes_animator);
        }
    }

    public override void FixedUpdateState()
    {
        // Go into running speed
        if (_helper.speedUpTimer > 0)
        {
            _helper.speedUpTimer -= Time.deltaTime;
        }
    }

    private void JumpBufferCheck()
    {
        if (_helper._movementVars.jumpCooldownTimer <= 0 && _helper._movementVars.jumpBufferTimer > 0)
        {
            _movement.OnJump();
            _helper.isJumpBuffered = false;
        }
    }


    public override void EnterState()
    {
        ModifyStateKey(this);

        // Movement conditionals
        _helper.canJump = true;

        // Animation
        _animator.ChangeAnimationState(_animator.BASESLIME_MOVING, _animator.baseSlime_animator);
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_MOVING, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, -0.112f));

        // Hitbox
        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        // Go into running speed
        if (_helper.isRunning == true)
        {
            _animator.ChangeAnimationState(_animator.EYES_SCARED, _animator.eyes_animator);
        }
        else
        {
            _animator.ChangeAnimationState(_animator.EYES_MOVING, _animator.eyes_animator);
        }
    }


    public override void ExitState()
    {
        // Movement conditionals
        _helper.canJump = false;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
