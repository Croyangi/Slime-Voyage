using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_UniqueIdle : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;
    [SerializeField] private bool canEmote = false;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Emote.RandomEmote.performed += OnGenerateRandomUniqueIdle;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        //// Unubscribes to Unity's input system
        playerInput.Emote.RandomEmote.performed -= OnGenerateRandomUniqueIdle;
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

    public override void EnterState()
    {
        ModifyStateKey(this);

        GenerateRandomUniqueIdle();

        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        canEmote = true;
    }


    public override void ExitState()
    {
        canEmote = false;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }

    private void OnGenerateRandomUniqueIdle(InputAction.CallbackContext value)
    {
        if (canEmote == true)
        {
            GenerateRandomUniqueIdle();
        }
    }

    private void GenerateRandomUniqueIdle()
    {
        StopAllCoroutines();

        int randomUniqueIdle = Random.Range(0, 1 + 1);
        switch (randomUniqueIdle)
        {
            case 0:
                _animator.ChangeAnimationState(_animator.BASESLIME_UNIQUEIDLE_SPIN, _animator.baseSlime_animator);
                break;
            case 1:
                _animator.ChangeAnimationState(_animator.BASESLIME_UNIQUEIDLE_STRETCH, _animator.baseSlime_animator);
                StartCoroutine(ReturnToIdle(1f));
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
