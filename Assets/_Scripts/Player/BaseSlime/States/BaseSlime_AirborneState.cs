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
    [SerializeField] private BaseSlime_Movement _movement;
    [SerializeField] private bool isTransitioning;

    [SerializeField] private bool isBufferTransition;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

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

    public override void UpdateState()
    {
        //if (_helper.isTouchingLeft && _helper.isTouchingRight && !isTransitioning)
        //{
        //    if (_helper._movementVars.processedInputMovement.x == 1)
        //    {
        //        // Switch to right
        //        _helper.stickingDirection.x = 1;
        //        if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
        //        {
        //            Debug.Log("SWITCH");
        //            TransitionToState(state);
        //        }
        //    }

        //    if (_helper._movementVars.processedInputMovement.x == -1)
        //    {
        //        // Switch to left
        //        _helper.stickingDirection.x = -1;
        //        if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
        //        {
        //            Debug.Log("SWITCH");
        //            TransitionToState(state);
        //        }
        //    }
        //}

        if (_helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                isBufferTransition = true;
                TransitionToState(state);
            }
        }

        // Instant sticking if already stuck before, but in opposite velocity to prevent same wall sticking
        // Touching Direction ABS > 0, cause 0 is still positive sign
        // X Velocity > x, cause 0 is still a positive sign // CANT CHECK VELOCITY BECAUSE U HIT A WALL FIRST


        if (!_helper.isGrounded && _helper.isPermanentlySticking && _helper.touchingDirection.x == _helper.nextWallJumpDirection && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                Debug.Log("INSTANT");
                TransitionToState(state);
            }
        }

        // Normal sticking, press on a wall, but only when falling
        if (!_helper.isGrounded && _helper.stickingDirection != Vector2.zero && _helper.rb.velocity.y < 3f && (_helper.touchingDirection.x == _helper._movementVars.processedInputMovement.x && Mathf.Abs(_helper._movementVars.processedInputMovement.x) > 0) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                Debug.Log("NOTGrounded: " + !_helper.isGrounded +
                    "\nIsPermaStick: " + _helper.isPermanentlySticking +
                    "\nOnNextWallJump: " + (_helper.touchingDirection.x == _helper.nextWallJumpDirection));
                //Debug.Break();


                //Debug.Log("NORMAL");
                TransitionToState(state);
            }
        }

        // Rising, Falling, and Midair Variants
        if (_helper.rb.velocity.y > 3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_RISING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, 0.6f));
            //_helper.col_slime.offset = new Vector2(0f, 0.3f);

        } else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
            //_helper.col_slime.offset = new Vector2(0f, 0.3f);

        } else if (Mathf.Abs(_helper.rb.velocity.y) < 3f && Mathf.Abs(_helper.rb.velocity.y) > 0.1f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));
            //_helper.col_slime.offset = new Vector2(0f, 0f);
        }

        // Top Speed Scared Eyes
        if (_helper.isTopVisualSpeed)
        {
            _animator.ChangeAnimationState(_animator.EYES_SCARED, _animator.eyes_animator);
        } else
        {
            _animator.ChangeAnimationState(_animator.EYES_AIRBORNE, _animator.eyes_animator);
        }

        // Immediate switch to Sticking if jumping on a wall
        if (!_helper.isGrounded && _helper.stickingDirection != Vector2.zero && _helper._movementVars.jumpBufferTimer > 0f && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    public override void FixedUpdateState()
    {
    }

    private void OnBufferPerformed(InputAction.CallbackContext value)
    {
        _helper._movementVars.jumpBufferTimer = _helper._movementVars.jumpBuffer;
        //Debug.Log("BUFFERED");

        // Immediate switch to Sticking if jumping on a wall
        if (!_helper.isGrounded && _helper.stickingDirection != Vector2.zero && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Sticking, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Movement conditionals
        isBufferTransition = false;
        _helper.canJumpBuffer = true;

        // Input
        playerInput.BaseSlime.Jump.performed += OnBufferPerformed;

        ////// Hitboxes
        //_helper.col_slime.offset = new Vector2(0f, -0.15f);
        //_helper.col_slime.size = new Vector2(1.8f, 1.2f);
        //_helper.col_slime.size = new Vector2(0.8f, 0.8f);

        // Set hitboxes
        _helper.col_touchingLeft.offset = new Vector2(-0.8f, 0f);
        _helper.col_touchingLeft.size = new Vector2(0.5f, 0.81f);
        _helper.col_touchingRight.offset = new Vector2(0.8f, 0f);
        _helper.col_touchingRight.size = new Vector2(0.5f, 0.81f);
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
            //_helper.col_slime.offset = new Vector2(0f, 0.64f);

        }
        else if (_helper.rb.velocity.y < -3f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_FALLING, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
            //_helper.col_slime.offset = new Vector2(0f, -0.15f);

        }
        else if (Mathf.Abs(_helper.rb.velocity.y) < 3f && Mathf.Abs(_helper.rb.velocity.y) > 0.1f)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_MIDAIR, _animator.baseSlime_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.051f));
            //_helper.col_slime.offset = new Vector2(0f, 0f);
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
        // Movement conditionals
        if (isBufferTransition)
        {
            _helper.canJump = true;
            isBufferTransition = false;
        }
        _helper.canJumpBuffer = false;

        // Flipping
        if (_helper.stickingDirection.x == 1)
        {
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _animator.FlipSprite(false);
        }

        // Input
        playerInput.BaseSlime.Jump.performed -= OnBufferPerformed;

        // OnEdge hitboxes re-enabled
        _helper.col_onEdgeLeft.gameObject.SetActive(true);
        _helper.col_onEdgeRight.gameObject.SetActive(true);
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
