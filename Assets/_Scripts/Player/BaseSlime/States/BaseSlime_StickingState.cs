using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_StickingState : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private BaseSlime_Movement _movement;

    [SerializeField] private bool isTransitioning;
    [SerializeField] private PlayerInput playerInput = null;

    private Vector2 prev_isGrounded_size;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        playerInput.Enable();
    }

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
                Debug.Log("NON FORCE");
            }
        }

        // Force IMMEDIATE cancel stick, down or opposite direction
        if (_helper._movementVars.processedInputMovement.y < 0 || (Mathf.Sign(_helper._movementVars.processedInputMovement.x) == Mathf.Sign(-(_helper.stickingDirection.x)) && Mathf.Abs(_helper._movementVars.processedInputMovement.x) > 0) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {

                TransitionToState(state);

                // Movement conditionals
                Debug.Log("FORCE");
                _helper.isPermanentlySticking = false;
            }
        }

        if (_helper.stickingDirection.x == 1)
        {
            _helper.col_slime.offset = new Vector2(0.11f, 0f);
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _helper.col_slime.offset = new Vector2(-0.11f, 0f);
            _animator.FlipSprite(false);
        }

        _helper._movementVars.coyoteJumpTimer = 0f;
    }

    private void OnWallJumpPerformed(InputAction.CallbackContext value)
    {
        _movement.OnWallJump();

        if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
        {
            TransitionToState(state);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Input
        playerInput.BaseSlime.Jump.performed += OnWallJumpPerformed;

        // Animation
        _animator.SetEyesActive(false);

        // Movement conditionals
        _helper.isPermanentlySticking = true;
        _helper.canWallJump = true;
        _helper.canStick = true;

        // Delay because Jely just LOVEEEESSS TRANSITIONS HUH
        _animator.ChangeAnimationState(_animator.BASESLIME_STICKINGTRANSITION, _animator.baseSlime_animator);
        StartCoroutine(StickingTransitionDelay());

        // Set hitbox
        _helper.col_slime.offset = new Vector2(0.13f, 0f);
        _helper.col_slime.size = new Vector2(1.5f, 1.5f);

        //// Ground Hitboxes
        // Save previous hitboxes
        prev_isGrounded_size = _helper.col_isGrounded.size;

        // Set hitboxes
        _helper.col_isGrounded.size = new Vector2(1.6f, _helper.col_isGrounded.size.y);
        //////

        // Edging and sticky ironically doesn't go together
        _helper.col_onEdgeLeft.gameObject.SetActive(false);
        _helper.col_onEdgeRight.gameObject.SetActive(false);

        // A little bit of help sticking to the wall
        Vector2 appliedVelocity = new Vector2(1f * Mathf.Sign(_helper.stickingDirection.x), 0f);
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

        // Jump Buffer
        if (_helper.isJumpBuffered)
        {
            _movement.OnWallJump();
            _helper.isJumpBuffered = false;

            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }
    }


    public override void ExitState()
    {
        StopAllCoroutines();

        // Input
        playerInput.BaseSlime.Jump.performed -= OnWallJumpPerformed;

        // Re-enable colliders
        _helper.col_onEdgeLeft.gameObject.SetActive(true);
        _helper.col_onEdgeRight.gameObject.SetActive(true);

        // Set hitboxes
        _helper.col_isGrounded.size = prev_isGrounded_size;

        // Movement conditionals
        _helper.stickingDirection.x = 0f;
        _helper.canWallJump = false;
        _helper.canStick = false;
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
