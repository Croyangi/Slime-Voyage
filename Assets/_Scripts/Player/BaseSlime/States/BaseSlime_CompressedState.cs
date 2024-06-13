using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_CompressedState : State
{
    [Header("State References")]
    [SerializeField] private BaseSlime_StateMachine _stateMachine;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;
    [SerializeField] private BaseSlime_AnimatorHelper _animator;
    [SerializeField] private bool isTransitioning;

    [SerializeField] private BoxCollider2D col_headDetect;
    [SerializeField] private TagsScriptObj tag_isSolidGround;
    [SerializeField] private TagsScriptObj tag_isPlatform;

    public override void UpdateState()
    {
        if ((!_helper.isGrounded || _helper._movementVars.processedInputMovement.y > -1f) && IsForcedCompressedDetect() == false && !isTransitioning)
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

        _animator.ChangeAnimationState(_animator.BASESLIME_COMPRESS, _animator.baseSlime_animator);
        _animator.SetEyesActive(true);
        _animator.ChangeAnimationState(_animator.EYES_COMPRESSED, _animator.eyes_animator);
        _animator.SetEyesOffset(new Vector2(0f, -0.344f));

        _helper.col_slime.offset = new Vector2(-0.03f, -0.333f);
        _helper.col_slime.size = new Vector2(1.8f, 0.817f);

        _helper._movementVars.movementSpeed = 0f;
        _helper._movementVars.jumpVelocityXAdd = 5f;
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

    private bool IsForcedCompressedDetect()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_headDetect, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if ((_tags.CheckTags(tag_isSolidGround.name) == true))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
