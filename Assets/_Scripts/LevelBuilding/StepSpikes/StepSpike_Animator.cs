using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Building Blocks")]
    [SerializeField] private StepSpike_Movement _movement;
    [SerializeField] private StepSpike_StateHandler _stateHandler;

    [Header("State References")]
    [SerializeField] private bool isSpriteFlippedX;
    [SerializeField] private string currentState;
    [SerializeField] private int currentPriority;
    [SerializeField] private float currentPriorityTime;

    const string STEPSPIKE_WALKING = "StepSpike_Walking";
    const string STEPSPIKE_AIRBORNE = "StepSpike_Airborne";

    private void Awake()
    {
        ChangeAnimationState(STEPSPIKE_WALKING);

        isSpriteFlippedX = _spriteRenderer.flipX;
    }

    private void FlipSprite(bool flipDirection)
    {
        _spriteRenderer.flipX = flipDirection;
    }

    private void ChangeAnimationState(string newState, int newPriority = 0)
    {
        if (currentState == newState)
        {
            return;
        }

        if (currentPriority > newPriority && currentPriorityTime > 0)
        {
            return;
        }

        _animator.Play(newState);
        currentState = newState;
        currentPriority = newPriority;
    }

    private void FixedUpdate()
    {
        IsWalkingUpdate();
        SpriteFlipUpdate();
    }

    private void IsWalkingUpdate()
    {
        if (_stateHandler.isGrounded)
        {
            ChangeAnimationState(STEPSPIKE_WALKING);
        } else
        {
            ChangeAnimationState(STEPSPIKE_AIRBORNE);
        }
    }

    private void SpriteFlipUpdate()
    {
        if (_rigidBody2D.velocity.x > 0.1)
        {
            FlipSprite(false);
        }
        else if (_rigidBody2D.velocity.x < -0.1)
        {
            FlipSprite(true);
        }

        if (_movement.horizontalMovement == 1)
        {
            FlipSprite(false);
        }
        else if (_movement.horizontalMovement == -1)
        {
            FlipSprite(true);
        }
    }
}
