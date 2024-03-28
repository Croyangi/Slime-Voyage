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
        if (_helper.isGrounded && _helper.stickingDirection.x == 0 && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }

        if (!_helper.isGrounded && _helper.stickingDirection.x == 0 && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.stickingDirection.x == 1)
        {
            _helper.col_slime.offset = new Vector2(0.11f, 0.5f);
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _helper.col_slime.offset = new Vector2(-0.11f, 0.5f);
            _animator.FlipSprite(false);
        }

        _helper._movementVars.coyoteJumpTimer = 0f;
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Delay because Jely just LOVEEEESSS TRANSITIONS HUH
        _animator.ChangeAnimationState(_animator.BASESLIME_STICKINGTRANSITION, _animator.baseSlime_animator);
        StartCoroutine(StickingTransitionDelay());

        // Set hitbox
        _helper.col_slime.offset = new Vector2(0.13f, 0.5f);
        _helper.col_slime.size = new Vector2(1.5f, 2.5f);

        // Edging and sticky ironically doesn't go together
        _helper.col_onEdgeLeft.gameObject.SetActive(false);
        _helper.col_onEdgeRight.gameObject.SetActive(false);

        // A little bit of help sticking to the wall
        Vector2 appliedVelocity = new Vector2(10f * Mathf.Sign(_helper.stickingDirection.x), 0f);
        _helper.rb.AddForce(appliedVelocity, ForceMode2D.Impulse);

        // Flip sprite before you even know you sticked, smooooth, and flip X offsets
        if (_helper.stickingDirection.x == 1)
        {
            _helper.col_slime.offset = new Vector2(0.11f, 0.5f);
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _helper.col_slime.offset = new Vector2(-0.11f, 0.5f);
            _animator.FlipSprite(false);
        }
    }


    public override void ExitState()
    {
        StopAllCoroutines();

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

    private IEnumerator StickingTransitionDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _animator.ChangeAnimationState(_animator.BASESLIME_STICKING, _animator.baseSlime_animator);
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_STICKING, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, -0.016f));

    }
}
