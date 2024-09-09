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

    [SerializeField] private TagsScriptObj tag_isVerticalCorrecting;

    private Vector2 prev_isGrounded_size;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        playerInput.Enable();
    }

    public override void UpdateState()
    {
        // Flipping
        if (_helper.stickingDirection.x == 1)
        {
            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _animator.FlipSprite(false);
        }

        //_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && _rigidBody2D.velocity.y < 0.1 && _stateHandler.isOnEdge == 0
        if (_helper.isGrounded && _helper.stickingDirection.x == 0 && !isTransitioning)
        {
            //Debug.Log("IDLE");

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
                //Debug.Log("NON FORCE");
            }
        }

        // Force IMMEDIATE cancel stick, down
        if (_helper._movementVars.processedInputMovement.y < 0 && _helper._movementVars.processedInputMovement.x == 0 && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {

                TransitionToState(state);

                // Movement conditionals
                //Debug.Log("CANCEL DOWN");
                _helper.isPermanentlySticking = false;
            }
        }

        // Immediate cancel stick, unless unstickable timer going on
        if ((Mathf.Sign(_helper._movementVars.processedInputMovement.x) == Mathf.Sign(-(_helper.stickingDirection.x)) && Mathf.Abs(_helper._movementVars.processedInputMovement.x) > 0) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {

                TransitionToState(state);

                // Movement conditionals
                //Debug.Log("CANCEL HORIZ");
                _helper.isPermanentlySticking = false;
            }
        }

        // Jump Buffer
        if (_helper._movementVars.jumpBufferTimer > 0)
        {
            _movement.OnWallJump();
            _helper.isJumpBuffered = false;

            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }


        _helper._movementVars.coyoteJumpTimer = 0f;
    }

    public override void FixedUpdateState()
    {
        // Unstickable timer
        _helper._movementVars.unstickableTimer = Mathf.Clamp(_helper._movementVars.unstickableTimer -= Time.fixedDeltaTime, 0, 9999);

        // Sticking Movement Math
        _helper.rb.velocity = new Vector2(1f * Mathf.Sign(_helper.stickingDirection.x), _helper.rb.velocity.y);
        _movement.StickingWallMovementMath();
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

        // Set hitboxes
        prev_isGrounded_size = _helper.col_isGrounded.size;
        _helper.col_isGrounded.size = new Vector2(1.6f, _helper.col_isGrounded.size.y);

        _helper.col_touchingLeft.offset = new Vector2(-0.82f, 0f);
        _helper.col_touchingLeft.size = new Vector2(0.5f, 0.8f);
        _helper.col_touchingRight.offset = new Vector2(0.82f, 0f);
        _helper.col_touchingRight.size = new Vector2(0.5f, 0.8f);
        //_helper.col_slime.offset = new Vector2(0f, 0.3f);
        //_helper.col_slime.size = new Vector2(1.8f, 1.2f);

        // To help with hitboxes and sprites
        if (_helper.stickingDirection.x == 1)
        {
            _helper.col_touchingLeft.offset = new Vector2(-0.43f, -0.19f);
            _helper.col_touchingLeft.size = new Vector2(0.5f, 0.8f);

            _animator.FlipSprite(true);
        }
        else if (_helper.stickingDirection.x == -1)
        {
            _helper.col_touchingRight.offset = new Vector2(0.43f, -0.19f);
            _helper.col_touchingRight.size = new Vector2(0.5f, 0.8f);

            _animator.FlipSprite(false);
        }

        // Re-affirm states
        _helper.PermanentlyStickingUpdate();

        // Confirm next wall jump direction to prevent state switching
        _helper.nextWallJumpDirection = 1 * ((int)Mathf.Sign(-_helper.stickingDirection.x));

        // Flip sprite before you even know you sticked, smooooth, and flip X offsets
        // Don't trigger natural velocity X sprite flipping
        //_helper.rb.velocity = new Vector2(0f, _helper.rb.velocity.y);

        // Unstickable set
        _helper._movementVars.unstickableTimer = _helper._movementVars.unstickableTime;
        _movement.SetMovementStall(_helper._movementVars.unstickableTime);

        // Input
        playerInput.BaseSlime.Jump.performed += OnWallJumpPerformed;

        // Movement conditionals
        _helper.isPermanentlySticking = true;
        _helper.canWallJump = true;
        _helper.canStick = true;

        // Edging and sticky ironically doesn't go together
        _helper.col_onEdgeLeft.gameObject.SetActive(false);
        _helper.col_onEdgeRight.gameObject.SetActive(false);

        // Jump Buffer
        if (_helper._movementVars.jumpBufferTimer > 0f)
        {
            _movement.OnWallJump();
            _helper.isJumpBuffered = false;

            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }
        else // ANYTHING COMES AFTER JUMP BUFFER, SHOULD BE ANIMATION WISE OR NON-JUMP BUFFER SETS
        {
            // Animation
            _animator.SetEyesActive(false);

            // Delay because Jely just LOVEEEESSS TRANSITIONS HUH
            _animator.ChangeAnimationState(_animator.BASESLIME_STICKINGTRANSITION, _animator.baseSlime_animator);
            StartCoroutine(StickingTransitionDelay());

            //CheckPenetrationDepth();
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
        _helper.stickingDirection.x = 0;
        _helper.canWallJump = false;
        _helper.canStick = false;
        _helper.stopPriorityStickDirection = false;
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

    // yo???????
    private void CheckPenetrationDepth()
    {
        float distance = _helper.col_slime.bounds.size.y / 2f;

        Vector3 topMidPos = new Vector3(transform.position.x + _helper.col_slime.offset.x, transform.position.y + _helper.col_slime.offset.y, 0f);
        RaycastHit2D[] midHits = Physics2D.RaycastAll(topMidPos, Vector2.down, distance);
        VerticalCorrectingRaycast(midHits);

        Vector3 topLeftPos = new Vector3(transform.position.x + _helper.col_slime.offset.x - _helper.col_slime.bounds.size.x / 2f, transform.position.y + _helper.col_slime.offset.y, 0f);
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(topLeftPos, Vector2.down, distance);
        VerticalCorrectingRaycast(leftHits);

        Vector3 topRightPos = new Vector3(transform.position.x + _helper.col_slime.offset.x + _helper.col_slime.bounds.size.x / 2f, transform.position.y + _helper.col_slime.offset.y, 0f);
        RaycastHit2D[] rightHits = Physics2D.RaycastAll(topRightPos, Vector2.down, distance);
        VerticalCorrectingRaycast(rightHits);
    }

    private void VerticalCorrectingRaycast(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null) // On Hit
            {

                //Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_isVerticalCorrecting.name) == true)
                    {
                        //Debug.Log("CORRECTED");
                        Vector3 pos = _helper.baseSlime.transform.position;

                        float bottomPos = transform.position.y + _helper.col_slime.offset.y - (_helper.col_slime.bounds.size.y / 2f);
                        float heightError = Mathf.Abs(hit.point.y - bottomPos);
                        _helper.baseSlime.transform.position = new Vector3(pos.x, pos.y + heightError, pos.z);
                        _helper.rb.velocity = new Vector2(_helper.rb.velocity.x, 0f);
                    }
                }
            }
        }
    }
}
