using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSlime_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("State References")]
    [SerializeField] private bool isSpriteFlippedX;
    [SerializeField] private string currentState;
    [SerializeField] private int currentPriority;
    [SerializeField] private float currentPriorityTime;

    const string GHOSTSLIME_IDLE = "GhostSlime_Idle";
    const string GHOSTSLIME_RISING = "GhostSlime_Rising";
    const string GHOSTSLIME_FALLING = "GhostSlime_Falling";

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float fade_speed = 1;
    [SerializeField] private float fade_minAlpha;
    [SerializeField] private float fade_maxAlpha;
    [SerializeField] private float fade_alphaMidpoint;
    [SerializeField] private float fade_alphaRange;


    private void FadeAlphaUpdate()
    {
        float alpha = fade_alphaMidpoint + fade_alphaRange * Mathf.Sin(Time.time * fade_speed);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, alpha);
    }

    [Header("Building Block References")]
    [SerializeField] private GhostSlime_MovementVariables _movementVars;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeAnimationState(GHOSTSLIME_IDLE, -1);

        isSpriteFlippedX = _spriteRenderer.flipX;

        // Set alpha variables
        fade_alphaMidpoint = (fade_minAlpha + fade_maxAlpha) / 2.0f;
        fade_alphaRange = (fade_maxAlpha - fade_minAlpha) / 2.0f;
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

    private void FlipSprite(bool flipDirection)
    {
        _spriteRenderer.flipX = flipDirection;
    }

    private void FixedUpdate()
    {
        // Fade in and out sprite
        FadeAlphaUpdate();

        if (currentPriorityTime > 0) { currentPriorityTime -= Time.deltaTime; }

        SpriteFlipUpdate();

        IsIdleUpdate();
        IsChangingYUpdate();
    }

    private void IsIdleUpdate()
    {
        if (_movementVars.processedInputMovement.y == 0)
        {
            ChangeAnimationState(GHOSTSLIME_IDLE, -1);
        }
    }

    private void IsChangingYUpdate()
    {
        if (_movementVars.processedInputMovement.y > 0)
        {
            ChangeAnimationState(GHOSTSLIME_RISING);
        } else if (_movementVars.processedInputMovement.y < 0)
        {
            ChangeAnimationState(GHOSTSLIME_FALLING);
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

        if (_movementVars.processedInputMovement.x == 1)
        {
            FlipSprite(false);
        }
        else if (_movementVars.processedInputMovement.x == -1)
        {
            FlipSprite(true);
        }
    }
}
