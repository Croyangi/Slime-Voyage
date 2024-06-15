using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class BaseSlime_Movement : MonoBehaviour, IMovementProcessor
{
    [Header("General References")]
    public Rigidbody2D rb; 

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]
    [SerializeField] private BaseSlime_MovementVariables _movementVars;
    [SerializeField] private BaseSlime_StateMachineHelper _helper;

    [Header("SFX")]
    [SerializeField] private AudioClip sfx_jump;

    [Header("Variables")]
    [SerializeField] public float jumpMovement; // From Unity's input system

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        _movementVars.rawInputMovement = Vector2.zero; // Prevents "sticky" inputs before the scene runs
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.BaseSlime.Movement.performed += OnMovementPerformed;
        playerInput.BaseSlime.Movement.canceled += OnMovementCancelled;
        playerInput.BaseSlime.Jump.performed += OnJumpPerformed;
        playerInput.BaseSlime.Jump.canceled += OnJumpCancelled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        //// Unubscribes to Unity's input system
        playerInput.BaseSlime.Movement.performed -= OnMovementPerformed;
        playerInput.BaseSlime.Movement.canceled -= OnMovementCancelled;
        playerInput.BaseSlime.Jump.performed -= OnJumpPerformed;
        playerInput.BaseSlime.Jump.canceled -= OnJumpCancelled;
        playerInput.Disable();
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _movementVars.rawInputMovement = value.ReadValue<Vector2>(); // Reads input as Vector2
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _movementVars.rawInputMovement = Vector2.zero; // Resets input
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        //// _stateHandler.stickingDirection == Vector2.zero
        // Basically acts as "Not Sticking", vice versa

        if (!_helper.isGrounded) // Jump buffer if not on the ground
        {
            _movementVars.jumpBufferTimer = _movementVars.jumpBuffer;
        }

        if (_helper.isGrounded && _helper.stickingDirection == Vector2.zero && Mathf.Abs(_helper.rb.velocity.y) < 3f && _movementVars.jumpCooldownTimer <= 0)
        {
            float jump = value.ReadValue<float>();
            jumpMovement = jump;

            OnJump();
            return;
        }

        if (!_helper.isGrounded && _movementVars.coyoteJumpTimer != 0 && _movementVars.coyoteJumpTimer < _movementVars.coyoteTime && _movementVars.jumpCooldownTimer <= 0)
        {
            float jump = value.ReadValue<float>();
            jumpMovement = jump;

            OnJump();
            return;
        }

        if (_movementVars.coyoteJumpTimer == 0 && _helper.stickingDirection != Vector2.zero)
        {
            SetWallJumpTechnicals();
            return;
        }
    }

    private void OnJump() 
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        Vector2 jumpVelocity = new Vector2(_movementVars.jumpVelocityXAdd * Mathf.Sign(_helper.facingDirection), _movementVars.jumpStrength);
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

        Manager_SFXPlayer.instance.PlaySFXClip(sfx_jump, transform, 0.1f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f, 1f, 1f, 30f, spread: 180);
        _movementVars.jumpCooldownTimer = _movementVars.jumpCooldown;
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        _movementVars.coyoteJumpTimer = 0;
        jumpMovement = 0f;
    }

    private void SetWallJumpTechnicals()
    {
        ApplyWallJumpForce();
        _movementVars.deccelerationTimer = 0.4f;
        _movementVars.movementStallTime = _movementVars.wallJumpStallTime;
    }

    private void ApplyWallJumpForce()
    {
        rb.velocity = Vector2.zero;

        Vector2 wallJumpVelocity = new Vector2(_movementVars.wallJumpStrengthHorizontal * -(_helper.stickingDirection.x), _movementVars.wallJumpStrengthVertical);
        rb.AddForce(wallJumpVelocity, ForceMode2D.Impulse);
        _helper.facingDirection = (int) -(_helper.stickingDirection.x);

        Manager_SFXPlayer.instance.PlaySFXClip(sfx_jump, transform, 0.1f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f, 1f, 1f, 30f, spread: 180);
    }

    private void CoyoteTimeUpdate()
    {
        if (_helper.isGrounded)
        {
            _movementVars.coyoteJumpTimer = _movementVars.coyoteTime;
        }
        else
        {
            _movementVars.coyoteJumpTimer = Mathf.Clamp(_movementVars.coyoteJumpTimer -= Time.fixedDeltaTime, 0, _movementVars.coyoteTime);
        }
    }

    private void JumpBufferUpdate()
    {
        if (_movementVars.jumpBufferTimer > 0)
        {
            _movementVars.jumpBufferTimer -= Time.fixedDeltaTime;
            if (_helper.isGrounded)
            {
                JumpBufferCheck();
            }

        }
    }

    private void JumpBufferCheck()
    {
        //// _stateHandler.stickingDirection == Vector2.zero
        // Basically acts as "Not Sticking", vice versa

        _movementVars.jumpBufferTimer = 0;

        if ((_helper.isGrounded && _helper.stickingDirection == Vector2.zero) || (!_helper.isGrounded && _movementVars.coyoteJumpTimer != 0 && _movementVars.coyoteJumpTimer < _movementVars.coyoteTime))
        {
            if (_movementVars.jumpCooldownTimer <= 0)
            {
                float jump = 1;
                jumpMovement = jump;

                OnJump();
            }
        }

        if (_movementVars.coyoteJumpTimer == 0 && _helper.stickingDirection != Vector2.zero)
        {
            SetWallJumpTechnicals();
        }
    }

    // Part of the IMovementProcessor interface
    public void SetMovementStall(float time)
    {
        _movementVars.movementStallTime = time;
    }

    // Part of the IMovementProcessor interface
    public void SetDecellerationStall(float time)
    {
        _movementVars.deccelerationTimer = time;
    }

    public void SetInputStall(bool state)
    {
        if (state)
        {
            //// Subscribes to Unity's input system
            playerInput.BaseSlime.Movement.performed += OnMovementPerformed;
            playerInput.BaseSlime.Movement.canceled += OnMovementCancelled;
            playerInput.BaseSlime.Jump.performed += OnJumpPerformed;
            playerInput.BaseSlime.Jump.canceled += OnJumpCancelled;
        } else
        {
            // Resets player movement
            _movementVars.rawInputMovement = Vector2.zero;

            //// Unubscribes to Unity's input system
            playerInput.BaseSlime.Movement.performed -= OnMovementPerformed;
            playerInput.BaseSlime.Movement.canceled -= OnMovementCancelled;
            playerInput.BaseSlime.Jump.performed -= OnJumpPerformed;
            playerInput.BaseSlime.Jump.canceled -= OnJumpCancelled;
        }
    }

    private void DecellerationStall()
    {
        _movementVars.groundedDeceleration = 1f;
        _movementVars.deccelerationTimer -= Time.fixedDeltaTime;
    }

    private void MovementInputProcessor()
    {
        
        _movementVars.processedInputMovement = new Vector2(Mathf.RoundToInt(_movementVars.rawInputMovement.x), Mathf.RoundToInt(_movementVars.rawInputMovement.y));

        if (_movementVars.isMovementStalled)
        {
            _movementVars.processedInputMovement = Vector2.zero;
        }
    }

    private void MovementStallUpdate()
    {
        if (_movementVars.movementStallTime > 0) 
        {
            _movementVars.isMovementStalled = true;
            _movementVars.movementStallTime = Mathf.Clamp(_movementVars.movementStallTime -= Time.fixedDeltaTime, 0, 9999);
        } else
        {
            _movementVars.isMovementStalled = false;
        }
    }

    private void DecellerationStallUpdate() // Reduces the Decelleration to travel further in the air
    {
        if (_movementVars.deccelerationTimer > 0 && _helper.isGrounded == false)
        {
            DecellerationStall();
        }
        else
        {
            _movementVars.groundedDeceleration = _movementVars.initialDeceleration;
        }
    }

    private void MainMovementMath()
    {
        // Math.Sign is because Unity's input can give float values if diagonal movement
        float targetSpeed = Math.Sign(_movementVars.processedInputMovement.x) * _movementVars.movementSpeed;
        float speedDif = targetSpeed - rb.velocity.x;

        //// Don't slow down if exceeding target speed in the same vector direction
        //if ((Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed)) && _stateHandler.isGrounded == false && (Math.Sign(_movementVars.processedInputMovement.x) == Math.Sign(rb.velocity.x)))
        //{
        //    Debug.Log("NMOW");
        //    speedDif = 0f - rb.velocity.x;
        //}

        // Acceleration
        float accelRate = 0f;
        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = _movementVars.acceleration;
        }
        else
        {
            accelRate = _movementVars.groundedDeceleration;
        }

        //// Airborne
        //if (_stateHandler.isGrounded == false && _movementVars.processedInputMovement != Vector2.zero && (Math.Sign(_movementVars.processedInputMovement.x) == Math.Sign(rb.velocity.x)))
        //{
        //    accelRate = _movementVars.airborneDeceleration;
        //}

        // Don't slow down if exceeding target speed in the same vector direction
        float movement = 0f;
        if ((Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed)) && _helper.isGrounded == false && (Math.Sign(_movementVars.processedInputMovement.x) == Math.Sign(rb.velocity.x)))
        {
            //speedDif = 0f - rb.velocity.x;
            accelRate = _movementVars.exceedDeceleration;
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.velocityPower) * Mathf.Sign(speedDif);
        } else if ((Mathf.Abs(rb.velocity.x) > Math.Abs(_movementVars.movementSpeed)) && _helper.isGrounded == false && _movementVars.processedInputMovement.x == 0f)
        {
            accelRate = _movementVars.exceedDeceleration;
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.velocityPower) * Mathf.Sign(speedDif);
        } else
        {
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.velocityPower) * Mathf.Sign(speedDif);
        }

        rb.AddForce(movement * Vector2.right);
    }

    private void StickingWallMovementMath()
    {
        float targetSpeed = _movementVars.stickingWallSpeed;
        float speedDif = targetSpeed - rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _movementVars.stickingWallAcceleration : _movementVars.stickingWallDeceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.stickingWallVelocityPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.up);
    }

    private void JumpCooldownUpdate()
    {
        if (_movementVars.jumpCooldownTimer > 0)
        {
            _movementVars.jumpCooldownTimer = Mathf.Clamp(_movementVars.jumpCooldownTimer -= Time.fixedDeltaTime, 0, 9999);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, _movementVars.maxFallSpeed, 99999));

        MovementInputProcessor(); // Processes Raw Input to Processed Input

        DecellerationStallUpdate(); // Updates Decelleration Stall clock

        MovementStallUpdate(); // Updates Movement Stall clock

        MainMovementMath(); // Main movement math

        if (_helper.stickingDirection != Vector2.zero) { StickingWallMovementMath(); } // Main sticking wall movement math

        CoyoteTimeUpdate(); // Coyote time update
        JumpBufferUpdate(); // Jump buffer update
        JumpCooldownUpdate(); // Jump cooldown update
    }
}
