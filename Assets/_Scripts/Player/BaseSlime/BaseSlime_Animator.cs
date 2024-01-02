using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Technical References")]
    [SerializeField] private float currentHighestImpactVelocityY;
    [SerializeField] private float uniqueIdleAnimationTimer;
    [SerializeField] private float uniqueIdleAnimationCooldown;

    [Header("State References")]

    [SerializeField] private bool isSpriteFlippedX;
    [SerializeField] private string currentState;
    [SerializeField] private int currentPriority;
    [SerializeField] private float currentPriorityTime;

    const string BASESLIME_COMPRESS = "BaseSlime_Compress";
    const string BASESLIME_IDLE = "BaseSlime_Idle";
    const string BASESLIME_IDLE_SLIMEPILLED = "BaseSlime_Idle_SlimePilled";
    const string BASESLIME_IDLE_SPIN = "BaseSlime_Idle_Spin";
    const string BASESLIME_IDLE_STRETCH = "BaseSlime_Idle_Stretch";
    const string BASESLIME_LIGHTSPLAT = "BaseSlime_LightSplat";
    const string BASESLIME_LOOKUP = "BaseSlime_LookUp";
    const string BASESLIME_MIDAIR = "BaseSlime_Midair";
    const string BASESLIME_MOVING = "BaseSlime_Moving";
    const string BASESLIME_FALLING = "BaseSlime_Falling";
    const string BASESLIME_RISING = "BaseSlime_Rising";
    const string BASESLIME_SPLAT = "BaseSlime_Splat";
    const string BASESLIME_STICK = "BaseSlime_Stick";
    const string BASESLIME_TEETER = "BaseSlime_Teeter";



    [Header("Building Block References")]
    [SerializeField] private BaseSlime_StateHandler _stateHandler;
    [SerializeField] private BaseSlime_MovementVariables _movementVars;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeAnimationState(BASESLIME_IDLE, -1);

        isSpriteFlippedX = _spriteRenderer.flipX;
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
        if (currentPriorityTime > 0) { currentPriorityTime -= Time.deltaTime; }

        SpriteFlipUpdate();

        IsIdleUpdate();
        UniqueIdleAnimationUpdate();

        SplatCheckUpdate(_rigidBody2D.velocity.y);

        IsTeeteringUpdate();

        IsLookingUpUpdate();
        IsCompressingUpdate();

        IsMovingUpdate();

        IsRisingUpdate();
        IsFallingUpdate();
        IsMidairUpdate();

        IsStickingUpdate();
    }

    private void UniqueIdleAnimationUpdate()
    {
        bool isStillY = false;
        bool isStillX = false;

        if (_rigidBody2D.velocity.y > -0.1 && _rigidBody2D.velocity.y < 0.1)
        {
            isStillY = true;
        }

        if (_rigidBody2D.velocity.x > -0.1 && _rigidBody2D.velocity.x < 0.1)
        {
            isStillX = true;
        }

        if (_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && isStillX && isStillY && _stateHandler.isOnEdge == 0)
        {
            uniqueIdleAnimationTimer += Time.deltaTime;
        } else if (uniqueIdleAnimationTimer > 0) 
        {
            currentPriorityTime = 0;
            uniqueIdleAnimationTimer = 0;
        } else
        {
            uniqueIdleAnimationTimer = 0;
        }

        if (uniqueIdleAnimationTimer >= uniqueIdleAnimationCooldown)
        {
            PlayUniqueIdleAnimation();
            uniqueIdleAnimationTimer = 0;
        }
    }

    private void PlayUniqueIdleAnimation()
    {
        int randomTemp = Random.Range(0, 2+1);

        if (randomTemp == 0)
        {
            currentPriorityTime = 5f;
            ChangeAnimationState(BASESLIME_IDLE_SLIMEPILLED);
        } else if (randomTemp == 1)
        {
            currentPriorityTime = 4f;
            ChangeAnimationState(BASESLIME_IDLE_SPIN);
        } else if (randomTemp == 2)
        {
            currentPriorityTime = 0.8f;
            ChangeAnimationState(BASESLIME_IDLE_STRETCH);
        }
    }

    private void SplatCheckUpdate(float newImpactVelocity = 0)
    {
        if (newImpactVelocity < currentHighestImpactVelocityY)
        { 
            currentHighestImpactVelocityY = newImpactVelocity;
        }

        if (currentHighestImpactVelocityY < -1 && currentHighestImpactVelocityY > -30 && _stateHandler.isGrounded && _rigidBody2D.velocity.y <= 0) 
        {
            currentHighestImpactVelocityY = 0;
            currentPriorityTime = 0.07f;
            ChangeAnimationState(BASESLIME_LIGHTSPLAT);
        } else if (currentHighestImpactVelocityY <= -30 && _stateHandler.isGrounded && _rigidBody2D.velocity.y <= 0)
        {
            currentHighestImpactVelocityY = 0;
            currentPriorityTime = 0.2f;
            ChangeAnimationState(BASESLIME_SPLAT, 0);
        }
    }

    private void SpriteFlipUpdate()
    {
        if (_rigidBody2D.velocity.x > 0.1)
        {
            FlipSprite(false);
        } else if (_rigidBody2D.velocity.x < -0.1)
        {
            FlipSprite(true);
        }

        if (_movementVars.processedInputMovement.x == 1)
        {
            FlipSprite(false);
        } else if (_movementVars.processedInputMovement.x == -1)
        {
            FlipSprite(true);
        }
    }

    private void IsIdleUpdate()
    {
        if (_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && _rigidBody2D.velocity.y < 0.1 && _stateHandler.isOnEdge == 0)
        {
            ChangeAnimationState(BASESLIME_IDLE, -1);
        }
    }

    private void IsMovingUpdate()
    {
        if (_movementVars.processedInputMovement.x != 0 && _stateHandler.isGrounded)
        {
            ChangeAnimationState(BASESLIME_MOVING);
        }
    }

    private void IsRisingUpdate()
    {
        if (_rigidBody2D.velocity.y > 3 && !_stateHandler.isGrounded && _stateHandler.stickingDirection == Vector2.zero)
        {
            ChangeAnimationState(BASESLIME_RISING);
        }
    }

    private void IsFallingUpdate()
    {
        if (_rigidBody2D.velocity.y < -3 && !_stateHandler.isGrounded && _stateHandler.stickingDirection == Vector2.zero)
        {
            ChangeAnimationState(BASESLIME_FALLING);
        }
    }

    private void IsMidairUpdate()
    {
        if (((_rigidBody2D.velocity.y < 3 && _rigidBody2D.velocity.y > 1) || (_rigidBody2D.velocity.y < -1 && _rigidBody2D.velocity.y > -3)) && !_stateHandler.isGrounded && _stateHandler.stickingDirection == Vector2.zero)
        {
            ChangeAnimationState(BASESLIME_MIDAIR);
        }
    }

    private void IsStickingUpdate()
    {
        if (_stateHandler.stickingDirection != Vector2.zero)
        {
            ChangeAnimationState(BASESLIME_STICK);
        }

        if (_stateHandler.stickingDirection.x == 1)
        {
            FlipSprite(true);
        }
        else if (_stateHandler.stickingDirection.x == -1)
        {
            FlipSprite(false);
        }
    }

    private void IsLookingUpUpdate()
    {
        if (_movementVars.processedInputMovement.y == 1 && _stateHandler.isGrounded)
        {
            ChangeAnimationState(BASESLIME_LOOKUP);
        }
    }

    private void IsCompressingUpdate()
    {
        if (_movementVars.processedInputMovement.y == -1 && _stateHandler.isGrounded)
        {
            ChangeAnimationState(BASESLIME_COMPRESS);
        }
    }

    private void IsTeeteringUpdate()
    {
        if (_movementVars.processedInputMovement == Vector2.zero && _stateHandler.isGrounded && _rigidBody2D.velocity.y < 0.1 && _stateHandler.isOnEdge != 0) 
        {
            ChangeAnimationState(BASESLIME_TEETER);
        }

        if (_stateHandler.isOnEdge == 1)
        {
            FlipSprite(true);
        }
        else if (_stateHandler.isOnEdge == -1)
        {
            FlipSprite(false);
        }
    }
}
