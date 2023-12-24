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
    [SerializeField] private BaseSlime_StateHandler _stateHandler;

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

        if (!_stateHandler.isGrounded) // Jump buffer if not on the ground
        {
            _movementVars.jumpBufferTimer = _movementVars.jumpBuffer;
        }

        if ((_stateHandler.isGrounded && _stateHandler.stickingDirection == Vector2.zero) || (!_stateHandler.isGrounded && _movementVars.coyoteJumpTimer != 0 && _movementVars.coyoteJumpTimer < _movementVars.coyoteTime))
        {
            float jump = value.ReadValue<float>();
            jumpMovement = jump;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * _movementVars.jumpStrength, ForceMode2D.Impulse);
        }

        if (_movementVars.coyoteJumpTimer == 0 && _stateHandler.stickingDirection != Vector2.zero)
        {
            SetWallJumpTechnicals();
        }
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
        rb.velocity = new Vector2(_movementVars.wallJumpStrengthHorizontal * -(_stateHandler.stickingDirection.x), _movementVars.wallJumpStrengthVerticle);
    }

    private void CoyoteTimeUpdate()
    {
        if (_stateHandler.isGrounded)
        {
            _movementVars.coyoteJumpTimer = _movementVars.coyoteTime;
        }
        else
        {
            _movementVars.coyoteJumpTimer = Mathf.Clamp(_movementVars.coyoteJumpTimer -= Time.deltaTime, 0, _movementVars.coyoteTime);
        }
    }

    private void JumpBufferUpdate()
    {
        if (_movementVars.jumpBufferTimer > 0)
        {
            _movementVars.jumpBufferTimer -= Time.deltaTime;
            if (_stateHandler.isGrounded)
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

        if ((_stateHandler.isGrounded && _stateHandler.stickingDirection == Vector2.zero) || (!_stateHandler.isGrounded && _movementVars.coyoteJumpTimer != 0 && _movementVars.coyoteJumpTimer < _movementVars.coyoteTime))
        {
            float jump = 1;
            jumpMovement = jump;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * _movementVars.jumpStrength, ForceMode2D.Impulse);
        }

        if (_movementVars.coyoteJumpTimer == 0 && _stateHandler.stickingDirection != Vector2.zero)
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

    private void DecellerationStall()
    {
        _movementVars.decceleration = 1f;
        _movementVars.deccelerationTimer -= Time.deltaTime;
    }

    private void MovementInputProcessor()
    {
        _movementVars.processedInputMovement = _movementVars.rawInputMovement;

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
            _movementVars.movementStallTime = Mathf.Clamp(_movementVars.movementStallTime -= Time.deltaTime, 0, 9999);
        } else
        {
            _movementVars.isMovementStalled = false;
        }
    }

    private void DecellerationStallUpdate() // Reduces the Decelleration to travel further in the air
    {
        if (_movementVars.deccelerationTimer > 0 && _stateHandler.isGrounded == false)
        {
            DecellerationStall();
        }
        else
        {
            _movementVars.decceleration = _movementVars.initialDecceleration;
        }
    }

    private void MainMovementMath()
    {
        float targetSpeed = _movementVars.processedInputMovement.x * _movementVars.movementSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _movementVars.acceleration : _movementVars.decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.velocityPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }

    private void StickingWallMovementMath()
    {
        float targetSpeed = _movementVars.stickingWallSpeed;
        float speedDif = targetSpeed - rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _movementVars.stickingWallAcceleration : _movementVars.stickingWallDecceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _movementVars.stickingWallVelocityPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.up);
    }

    private void FixedUpdate()
    {
        MovementInputProcessor(); // Processes Raw Input to Processed Input

        DecellerationStallUpdate(); // Updates Decelleration Stall clock

        MovementStallUpdate(); // Updates Movement Stall clock

        MainMovementMath(); // Main movement math

        if (_stateHandler.stickingDirection != Vector2.zero) { StickingWallMovementMath(); } // Main sticking wall movement math

        CoyoteTimeUpdate(); // Coyote time update
        JumpBufferUpdate(); // Jump buffer update
    }
}
