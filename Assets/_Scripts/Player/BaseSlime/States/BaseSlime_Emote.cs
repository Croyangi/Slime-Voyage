using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_Emote : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("SFX")]
    [SerializeField] private AudioClip sfx_magicSparkle;

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
        if ((!_helper.isGrounded || _helper._movementVars.processedInputMovement != Vector2.zero) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }
    }

    public override void FixedUpdateState()
    {
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        // Emote Input
        playerInput.Emote.Emote0.performed += OnEmote0Performed;
        playerInput.Emote.Emote1.performed += OnEmote1Performed;
        playerInput.Emote.Emote2.performed += OnEmote2Performed;

        // Animation
        _animator.SetEyesActive(false);

        // Hitbox
        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        // Movement conditionals
        _helper.canJump = true;
        _helper.canEmote = true;

        // After canEmote = true
        ProcessEmote(_helper.emoteIndex);
    }


    public override void ExitState()
    {
        StopAllCoroutines();

        // Emote Input
        playerInput.Emote.Emote0.performed -= OnEmote0Performed;
        playerInput.Emote.Emote1.performed -= OnEmote1Performed;
        playerInput.Emote.Emote2.performed -= OnEmote2Performed;

        // Movement conditionals
        _helper.canJump = false;
        _helper.canEmote = false;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }

    private void OnEmotePerformed()
    {
        if (_helper.canEmote)
        {
            ProcessEmote(_helper.emoteIndex);
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

    private void ProcessEmote(int emoteIndex)
    {
        StopAllCoroutines();

        switch (emoteIndex)
        {
            case 0:
                _animator.ChangeAnimationState(_animator.BASESLIME_EMOTE_SPIN, _animator.baseSlime_animator);
                break;
            case 1:
                _animator.ChangeAnimationState(_animator.BASESLIME_EMOTE_STRETCH, _animator.baseSlime_animator);
                StartCoroutine(ReturnToIdle(1f));
                break;
            case 2:
                _animator.ChangeAnimationState(_animator.BASESLIME_EMOTE_CATEARS, _animator.baseSlime_animator);
                Manager_SFXPlayer.instance.PlaySFXClip(sfx_magicSparkle, transform, 1f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f, 1f, 1f, 30f, spread: 180);
                break;
        }
    }

    private IEnumerator ReturnToIdle(float time)
    {
        yield return new WaitForSeconds(time);
        if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
        {
            TransitionToState(state);
        }
    }
}
