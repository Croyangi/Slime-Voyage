using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_IdleState : State
{

    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private BaseSlime_Movement _movement;
    [SerializeField] private bool isTransitioning;

    [SerializeField] private bool isVerticalCorrecting = false;

    [SerializeField] private TagsScriptObj tag_isVerticalCorrecting;

    [Header("SFX")]
    [SerializeField] private AudioClip sfx_land;
    [SerializeField] private AudioClip sfx_landHard;

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
        playerInput.Emote.Emote0.performed -= OnEmote0Performed;
        playerInput.Emote.Emote1.performed -= OnEmote1Performed;
        playerInput.Emote.Emote2.performed -= OnEmote2Performed;
        playerInput.Disable();
    }

    public override void UpdateState()
    {
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

        if (_helper.isOnEdge != 0 && _helper._movementVars.processedInputMovement == Vector2.zero && _helper.isGrounded)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.OnEdge, out State state))
            {
                TransitionToState(state);
            }
        }

        // Jump Buffer
        if (_helper.isJumpBuffered)
        {
            JumpBufferCheck();
        }
    }

    public override void FixedUpdateState()
    {
    }

    private void JumpBufferCheck()
    {
        if (_helper._movementVars.jumpCooldownTimer <= 0 && _helper._movementVars.jumpBufferTimer > 0)
        {
            _movement.OnJump();
            _helper.isJumpBuffered = false;
            _movement.jumpChecker.color = Color.green;

            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Airborne, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Set hitboxes
        // Set hitboxes
        _helper.col_touchingLeft.offset = new Vector2(-0.8f, 0f);
        _helper.col_touchingLeft.size = new Vector2(0.5f, 0.81f);
        _helper.col_touchingRight.offset = new Vector2(0.8f, 0f);
        _helper.col_touchingRight.size = new Vector2(0.5f, 0.81f);

        // Emote Input
        playerInput.Emote.Emote0.performed += OnEmote0Performed;
        playerInput.Emote.Emote1.performed += OnEmote1Performed;
        playerInput.Emote.Emote2.performed += OnEmote2Performed;

        // Hitbox
        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        // Movement conditionals
        _helper._movementVars.movementSpeed = 10f;
        _helper._movementVars.jumpVelocityXAdd = 0f;
        _helper.canEmote = true;
        _helper.canJump = true;
        _helper.canJumpBuffer = true;

        // Vertical Correcting
        if (_helper.currentHighestImpactVelocityY < -0.1f && isVerticalCorrecting == true)
        {
            CheckPenetrationDepth();
        }

        // Plays landing animation based on Y velocity
        OnLandingAnimation();

        // Jump Buffer
        if (_helper.isJumpBuffered)
        {
            JumpBufferCheck();
        }
    }


    public override void ExitState()
    {
        StopAllCoroutines();

        // Emote Input
        playerInput.Emote.Emote0.performed -= OnEmote0Performed;
        playerInput.Emote.Emote1.performed -= OnEmote1Performed;
        playerInput.Emote.Emote2.performed -= OnEmote2Performed;

        // Movement Conditionals
        _helper.canEmote = false;
        _helper.canJump = false;
        _helper.canJumpBuffer = false;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Vector3 topPos = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + col_hitbox.bounds.size.y / 2f, 0f);
        Vector3 topMidPos = new Vector3(transform.position.x + _helper.col_slime.offset.x, transform.position.y + _helper.col_slime.offset.y, 0f);
        Vector3 topLeftPos = new Vector3(transform.position.x + _helper.col_slime.offset.x - _helper.col_slime.bounds.size.x / 2f, transform.position.y + _helper.col_slime.offset.y, 0f);
        Vector3 topRightPos = new Vector3(transform.position.x + _helper.col_slime.offset.x + _helper.col_slime.bounds.size.x / 2f, transform.position.y + _helper.col_slime.offset.y, 0f);
        Vector3 botPos = new Vector3(transform.position.x + _helper.col_slime.offset.x, transform.position.y + _helper.col_slime.offset.y - _helper.col_slime.bounds.size.y / 2f, 0f);


        //Gizmos.DrawWireSphere(col_hitbox.transform.position + topPos, 0.2f);
        //Gizmos.DrawWireSphere(col_hitbox.transform.position + botPos, 0.2f);

        float distance = _helper.col_slime.bounds.size.y / 2f;

        // Perform the raycast
        RaycastHit2D[] hits0 = Physics2D.RaycastAll(topMidPos, Vector2.down, distance);
        RaycastHit2D[] hits1 = Physics2D.RaycastAll(topLeftPos, Vector2.down, distance);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(topRightPos, Vector2.down, distance);

        List<RaycastHit2D> allHits = new List<RaycastHit2D>();
        allHits.AddRange(hits0);
        allHits.AddRange(hits1);
        allHits.AddRange(hits2);

        // Draw the raycast
        foreach (RaycastHit2D hit in hits0)
        {
            // Draw a line from topPos to the hit point
            Gizmos.DrawLine(topMidPos, hit.point);

            if (hit.collider != null) // On Hit
            {

                //float distanceError = Mathf.Abs(hit.point.y - transform.position.y);
                //Vector3 place = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + distanceError, 0f);
                //Gizmos.DrawWireSphere(place, 1f);
                //Debug.Log(distanceError);

                if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_isVerticalCorrecting.name) == true)
                    {
                        Vector3 here = new Vector3(hit.point.x, hit.point.y, 0f);
                        Gizmos.DrawWireSphere(here, 0.5f);
                    }
                }
            }
        }

        // Draw the raycast
        foreach (RaycastHit2D hit in hits1)
        {
            // Draw a line from topPos to the hit point
            Gizmos.DrawLine(topLeftPos, hit.point);

            if (hit.collider != null) // On Hit
            {

                //float distanceError = Mathf.Abs(hit.point.y - transform.position.y);
                //Vector3 place = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + distanceError, 0f);
                //Gizmos.DrawWireSphere(place, 1f);
                //Debug.Log(distanceError);

                if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_isVerticalCorrecting.name) == true)
                    {
                        Vector3 here = new Vector3(hit.point.x, hit.point.y, 0f);
                        Gizmos.DrawWireSphere(here, 0.5f);
                    }
                }
            }
        }

        // Draw the raycast
        foreach (RaycastHit2D hit in hits2)
        {
            // Draw a line from topPos to the hit point
            Gizmos.DrawLine(topRightPos, hit.point);

            if (hit.collider != null) // On Hit
            {

                //float distanceError = Mathf.Abs(hit.point.y - transform.position.y);
                //Vector3 place = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + distanceError, 0f);
                //Gizmos.DrawWireSphere(place, 1f);
                //Debug.Log(distanceError);

                if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_isVerticalCorrecting.name) == true)
                    {
                        Vector3 here = new Vector3(hit.point.x, hit.point.y, 0f);
                        Gizmos.DrawWireSphere(here, 0.5f);
                    }
                }
            }
        }
    }

    private void OnLandingAnimation()
    {
        StopAllCoroutines();

        if (_helper.currentHighestImpactVelocityY < -5 && _helper.currentHighestImpactVelocityY > -30)
        {
            //_animator.ChangeAnimationState(_animator.BASESLIME_LIGHTSPLAT, _animator.baseSlime_animator);
            //StartCoroutine(ReturnToIdleAnimation(0.05f));


            _animator.ChangeAnimationState(_animator.BASESLIME_IDLE, _animator.baseSlime_animator);
            _animator.SetEyesActive(true);
            _animator.ChangeAnimationState(_animator.EYES_IDLE, _animator.eyes_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));

            //Manager_SFXPlayer.instance.PlaySFXClip(sfx_land, transform, 0.5f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f);

        }
        else if (_helper.currentHighestImpactVelocityY <= -30)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_SPLAT, _animator.baseSlime_animator);
            _animator.SetEyesActive(false);
            StartCoroutine(ReturnToIdleAnimation(0.5f));

            Manager_SFXPlayer.instance.PlaySFXClip(sfx_landHard, transform, 0.5f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f, 1f, 1f, 30f, spread: 180);
            Manager_Cinemachine.instance.ApplyScreenShake(0.2f, 1f);
        }
        else
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_IDLE, _animator.baseSlime_animator);
            _animator.SetEyesActive(true);
            _animator.ChangeAnimationState(_animator.EYES_IDLE, _animator.eyes_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
        }

        _helper.currentHighestImpactVelocityY = 0f;
    }

    private IEnumerator ReturnToIdleAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.ChangeAnimationState(_animator.BASESLIME_IDLE, _animator.baseSlime_animator);
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_IDLE, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, -0.112f));
    }

    private void OnEmotePerformed()
    {
        if (_helper.isGrounded && !isTransitioning && _helper.canEmote == true)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Emote, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    private void OnEmote0Performed(InputAction.CallbackContext value)
    {
        _helper.emoteIndex = 0;
        OnEmotePerformed();
    }

    private void OnEmote1Performed(InputAction.CallbackContext value)
    {
        _helper.emoteIndex = 1;
        OnEmotePerformed();
    }

    private void OnEmote2Performed(InputAction.CallbackContext value)
    {
        _helper.emoteIndex = 2;
        OnEmotePerformed();
    }
}
