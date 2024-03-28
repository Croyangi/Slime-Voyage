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

        if (_helper._movementVars.processedInputMovement == Vector2.zero && _helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper._movementVars.processedInputMovement != Vector2.zero && _helper.isGrounded && !isTransitioning)
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
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, 0.6f));

            if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(0.3f, 0.64f);
            }
            else if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(-0.3f, 0.64f);
            }
        } else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));

            if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(0.3f, -0.15f);
            }
            else if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(-0.3f, -0.15f);
            }
        } else if (Mathf.Abs(_helper.rb.velocity.y) < 3f && Mathf.Abs(_helper.rb.velocity.y) > 0.1f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));

            _helper.col_slime.offset = new Vector2(0f, 0f);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Hitboxes
        _helper.col_slime.offset = new Vector2(0.3f, -0.15f);
        _helper.col_slime.size = new Vector2(1.2f, 1.2f);

        // No edge detection whilst airborne
        _helper.col_onEdgeLeft.gameObject.SetActive(false);
        _helper.col_onEdgeRight.gameObject.SetActive(false);

        // Animation
        if (_helper.rb.velocity.y > 3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, 0.6f));

            if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(0.3f, 0.64f);
            }
            else if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(-0.3f, 0.64f);
            }
        }
        else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));

            if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(0.3f, -0.15f);
            }
            else if (_helper.facingDirection == 1)
            {
                _helper.col_slime.offset = new Vector2(-0.3f, -0.15f);
            }
        } else
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));
            _helper.col_slime.offset = new Vector2(0f, 0f);
        }

        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_AIRBORNE, _animator.eyes_animator);
    }


    public override void ExitState()
    {
        _helper.col_onEdgeLeft.gameObject.SetActive(true);
        _helper.col_onEdgeRight.gameObject.SetActive(true);

        _animator.SetEyesActive(false);
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
