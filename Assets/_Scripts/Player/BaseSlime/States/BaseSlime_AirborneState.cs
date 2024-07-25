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
    [SerializeField] private PlayerInput playerInput = null;

    private Vector2 prev_touchingLeft_size;
    private Vector2 prev_touchingLeft_offset;
    private Vector2 prev_touchingRight_size;
    private Vector2 prev_touchingRight_offset;

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Enable();
    }

    private void OnDisable()
    {
        //// Unubscribes to Unity's input system
        playerInput.Disable();
    }

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

    public override void UpdateState()
    {

        if (_helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }


        // Normal sticking, press on a wall, but only when falling
        if (!_helper.isGrounded && _helper.stickingDirection != Vector2.zero && _helper.rb.velocity.y < 3f && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                Debug.Log("NORMAL");
                TransitionToState(state);
            }
        }

        // Instant sticking if already stuck before, but in opposite velocity to prevent same wall sticking
        // Touching Direction ABS > 0, cause 0 is still positive sign
        // X Velocity > x, cause 0 is still a positive sign
        // X Velocity check for splat impact
        if (_helper.rb.velocity.x > 30f)
        {
            _helper.isPermanentlySticking = true;
        }


        if (!_helper.isGrounded && _helper.isPermanentlySticking && (Mathf.Sign(_helper.rb.velocity.x) == Mathf.Sign(_helper.touchingDirection.x)) && Mathf.Abs(_helper.touchingDirection.x) > 0f && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                Debug.Log("INSTANT");
                TransitionToState(state);
            }
        }

        // Rising, Falling, and Midair Variants
        if (_helper.rb.velocity.y > 3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, 0.6f));
            _helper.col_slime.offset = new Vector2(0f, 0.3f);

        } else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
            _helper.col_slime.offset = new Vector2(0f, 0.3f);

        } else if (Mathf.Abs(_helper.rb.velocity.y) < 3f && Mathf.Abs(_helper.rb.velocity.y) > 0.1f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));
            _helper.col_slime.offset = new Vector2(0f, 0f);
        }

        // Top Speed Scared Eyes
        if (_helper.isTopVisualSpeed)
        {
            _animator.ChangeAnimationState(_animator.EYES_SCARED, _animator.eyes_animator);
        } else
        {
            _animator.ChangeAnimationState(_animator.EYES_AIRBORNE, _animator.eyes_animator);
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        _helper._movementVars.jumpBufferTimer = _helper._movementVars.jumpBuffer;
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Input
        playerInput.BaseSlime.Jump.performed += OnJumpPerformed;

        ////// Hitboxes
        _helper.col_slime.offset = new Vector2(0f, -0.15f);
        _helper.col_slime.size = new Vector2(1.8f, 1.2f);
        //_helper.col_slime.size = new Vector2(0.8f, 0.8f);

        //// Touching Side Hitboxes
        // Save previous hitboxes
        prev_touchingLeft_offset = _helper.col_touchingLeft.offset;
        prev_touchingLeft_size = _helper.col_touchingLeft.size;
        prev_touchingRight_offset = _helper.col_touchingRight.offset;
        prev_touchingRight_size = _helper.col_touchingRight.size;

        // Set hitboxes
        _helper.col_touchingLeft.offset = new Vector2(-0.8f, 0f);
        _helper.col_touchingLeft.size = new Vector2(0.5f, 0.5f);
        _helper.col_touchingRight.offset = new Vector2(0.8f, 0f);
        _helper.col_touchingRight.size = new Vector2(0.5f, 0.5f);
        //////

        // No edge detection whilst airborne
        _helper.col_onEdgeLeft.gameObject.SetActive(false);
        _helper.col_onEdgeRight.gameObject.SetActive(false);

        // Animation
        // Rising, Falling, and Midair Variants
        if (_helper.rb.velocity.y > 3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, 0.6f));
            _helper.col_slime.offset = new Vector2(0f, 0.64f);

        }
        else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
            _helper.col_slime.offset = new Vector2(0f, -0.15f);

        }
        else if (Mathf.Abs(_helper.rb.velocity.y) < 3f && Mathf.Abs(_helper.rb.velocity.y) > 0.1f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));
            _helper.col_slime.offset = new Vector2(0f, 0f);
        }

        // Eyes
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_AIRBORNE, _animator.eyes_animator);

        if (_helper.isTopVisualSpeed)
        {
            _animator.ChangeAnimationState(_animator.EYES_SCARED, _animator.eyes_animator);
        }
    }


    public override void ExitState()
    {
        // Input
        playerInput.BaseSlime.Jump.performed -= OnJumpPerformed;

        // OnEdge hitboxes re-enabled
        _helper.col_onEdgeLeft.gameObject.SetActive(true);
        _helper.col_onEdgeRight.gameObject.SetActive(true);

        // Set touching hitboxes back to saved hitboxes
        _helper.col_touchingLeft.offset = prev_touchingLeft_offset;
        _helper.col_touchingLeft.size = prev_touchingLeft_size;
        _helper.col_touchingRight.offset = prev_touchingRight_offset;
        _helper.col_touchingRight.size = prev_touchingRight_size;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
