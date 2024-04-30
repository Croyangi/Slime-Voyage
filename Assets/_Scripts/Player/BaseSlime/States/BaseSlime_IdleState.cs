using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovementScriptObj;

public class BaseSlime_IdleState : State
{

    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    [SerializeField] private Collider2D col_hitbox;

    [SerializeField] private bool canEmote = false;

    [SerializeField] private TagsScriptObj tag_isSolidGround;

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
        playerInput.Emote.RandomEmote.performed += OnRandomEmote;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        //// Unubscribes to Unity's input system
        playerInput.Emote.RandomEmote.performed -= OnRandomEmote;
        playerInput.Disable();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Vector3 topPos = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + col_hitbox.bounds.size.y / 2f, 0f);
    //    Vector3 botPos = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y - col_hitbox.bounds.size.y / 2f, 0f);


    //    //Gizmos.DrawWireSphere(col_hitbox.transform.position + topPos, 0.2f);
    //    //Gizmos.DrawWireSphere(col_hitbox.transform.position + botPos, 0.2f);

    //    float distance = col_hitbox.bounds.size.y;

    //    // Perform the raycast
    //    RaycastHit2D[] hits = Physics2D.RaycastAll(topPos, Vector2.down, distance);

    //    // Draw the raycast
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        // Draw a line from topPos to the hit point
    //        Gizmos.DrawLine(topPos, hit.point);

    //        if (hit.collider != null) // On Hit
    //        {

    //            //float distanceError = Mathf.Abs(hit.point.y - transform.position.y);
    //            //Vector3 place = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + distanceError, 0f);
    //            //Gizmos.DrawWireSphere(place, 1f);
    //            //Debug.Log(distanceError);

    //            if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
    //            {
    //                if (_tags.CheckTags(tag_isSolidGround.name) == true)
    //                {
    //                    Vector3 here = new Vector3(transform.position.x, hit.point.y, 0f);
    //                    Gizmos.DrawWireSphere(here, 0.5f);
    //                }
    //            }
    //        }
    //    }
    //}

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
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        _helper._movementVars.movementSpeed = 10f;
        _helper._movementVars.jumpVelocityXAdd = 0f;

        canEmote = true;

        if (_helper.currentHighestImpactVelocityY < -0.1f)
        {
            CheckPenetrationDepth();
        }

        OnLandingAnimation();
    }


    public override void ExitState()
    {
        StopAllCoroutines();

        canEmote = false;
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
        Vector3 topPos = new Vector3(transform.position.x + col_hitbox.offset.x, transform.position.y + col_hitbox.offset.y + col_hitbox.bounds.size.y / 2f, 0f);

        float distance = col_hitbox.bounds.size.y;

        RaycastHit2D[] hits = Physics2D.RaycastAll(topPos, Vector2.down, distance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null) // On Hit
            {

                //Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject.TryGetComponent<Tags>(out var _tags))
                {
                    if (_tags.CheckTags(tag_isSolidGround.name) == true)
                    {
                        Vector3 pos = _helper.baseSlime.transform.position;

                        float bottomPos = transform.position.y + col_hitbox.offset.y - (col_hitbox.bounds.size.y / 2f);
                        float heightError = Mathf.Abs(hit.point.y - bottomPos);
                        _helper.baseSlime.transform.position = new Vector3(pos.x, pos.y + heightError, pos.z);
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

            Manager_SFXPlayer.instance.PlaySFXClip(sfx_landHard, transform, 0.5f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f);
        } else
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

    private void OnRandomEmote(InputAction.CallbackContext value)
    {
        if (_helper.isGrounded && !isTransitioning && canEmote == true)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.UniqueIdle, out State state))
            {
                TransitionToState(state);
            }
        }
    }
}
