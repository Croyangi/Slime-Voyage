using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_LookingUp : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    public override void UpdateState()
    {
        if ((!_helper.isGrounded || _helper._movementVars.processedInputMovement.y < 1f) && !isTransitioning)
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

        _animator.ChangeAnimationState(_animator.BASESLIME_LOOKINGUP, _animator.baseSlime_animator);
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_LOOKINGUP, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, 0.5f));

        _helper.col_slime.offset = new Vector2(0, -0.058f);
        _helper.col_slime.size = new Vector2(1.8f, 1.37f);

        _helper._movementVars.movementSpeed = 0f;
    }


    public override void ExitState()
    {
        _helper._movementVars.movementSpeed = _helper._movementVars.walkingSpeed;
    }

    public override void TransitionToState(State state)
    {
        isTransitioning = true;
        ExitState();
        ModifyStateKey(state);
        isTransitioning = false;
    }
}
