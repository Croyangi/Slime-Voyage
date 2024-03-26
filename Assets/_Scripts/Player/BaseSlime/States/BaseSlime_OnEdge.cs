using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseSlime_OnEdge : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        if ((!_helper.isGrounded || _helper._movementVars.processedInputMovement != Vector2.zero || _helper.isOnEdge == 0) && !isTransitioning)
        {
            if (_stateMachine.PlayerStatesDictionary.TryGetValue(BaseSlime_StateMachine.PlayerStates.Idle, out State state))
            {
                TransitionToState(state);
            }
        }

        if (_helper.isOnEdge == 1)
        {
            _animator.FlipSprite(true);
        }
        else if (_helper.isOnEdge == -1)
        {
            _animator.FlipSprite(false);
        }
    }

    public override void EnterState()
    {
        ModifyStateKey(this);

        _animator.ChangeAnimationState(_animator.BASESLIME_ONEDGE, _animator.baseSlime_animator);
        _animator.ChangeAnimationState(_animator.EYES_ONEDGE, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, -0.058f));
        _animator.SetEyesActive(true);

        if (_helper.isOnEdge == 1)
        {
            _animator.FlipSprite(true);
        }
        else if (_helper.isOnEdge == -1)
        {
            _animator.FlipSprite(false);
        }

        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);
    }


    public override void ExitState()
    {
        _animator.SetEyesActive(false);
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
