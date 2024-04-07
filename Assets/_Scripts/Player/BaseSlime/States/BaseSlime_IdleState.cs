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
    [SerializeField] private bool isTransitioning;

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

        OnLandingAnimation();

        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        _helper._movementVars.movementSpeed = 10f;
        _helper._movementVars.jumpVelocityXAdd = 0f;
    }


    public override void ExitState()
    {
        _animator.SetEyesActive(false);
        StopAllCoroutines();
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }

    private void OnLandingAnimation()
    {
        StopAllCoroutines();

        if (_helper.currentHighestImpactVelocityY < -1 && _helper.currentHighestImpactVelocityY > -30)
        {
            //_animator.ChangeAnimationState(_animator.BASESLIME_LIGHTSPLAT, _animator.baseSlime_animator);
            //StartCoroutine(ReturnToIdleAnimation(0.05f));
            _helper.currentHighestImpactVelocityY = 0;

            _animator.ChangeAnimationState(_animator.BASESLIME_IDLE, _animator.baseSlime_animator);
            _animator.SetEyesActive(true);
            _animator.ChangeAnimationState(_animator.EYES_IDLE, _animator.eyes_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
        }
        else if (_helper.currentHighestImpactVelocityY <= -30)
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_SPLAT, _animator.baseSlime_animator);
            StartCoroutine(ReturnToIdleAnimation(0.5f));
            _helper.currentHighestImpactVelocityY = 0;
        } else
        {
            _animator.ChangeAnimationState(_animator.BASESLIME_IDLE, _animator.baseSlime_animator);
            _animator.SetEyesActive(true);
            _animator.ChangeAnimationState(_animator.EYES_IDLE, _animator.eyes_animator);
            _animator.SetEyesOffset(new Vector2(0f, -0.112f));
        }
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
        if (_helper.isGrounded && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.UniqueIdle, out State state))
            {
                TransitionToState(state);
            }
        }
    }
}
