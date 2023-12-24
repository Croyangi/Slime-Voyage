using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostSlime_Movement : MonoBehaviour, IMovementProcessor
{
    [Header("General References")]
    public Rigidbody2D rb;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]
    [SerializeField] private GhostSlime_MovementVariables _movementVars;

    [Header("Variables")]
    [SerializeField] private float ghostSlime_rotation;
    [SerializeField] private float ghostSlime_rotationSpeed;
    [SerializeField] private float ghostSlime_rotationReturnSpeed;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        _movementVars.rawInputMovement = Vector2.zero; // Prevents "sticky" inputs before the scene runs
    }

    private void OnEnable()
    {
        playerInput.GhostSlime.Movement.performed += OnMovementPerformed;
        playerInput.GhostSlime.Movement.canceled += OnMovementCancelled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.GhostSlime.Movement.performed -= OnMovementPerformed;
        playerInput.GhostSlime.Movement.canceled -= OnMovementCancelled;
        playerInput.Disable();
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

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _movementVars.rawInputMovement = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _movementVars.rawInputMovement = Vector2.zero;
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
        }
        else
        {
            _movementVars.isMovementStalled = false;
        }
    }

    private void DecellerationStallUpdate() // Reduces the Decelleration to travel further
    {
        if (_movementVars.deccelerationTimer > 0)
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
        float targetSpeedX = _movementVars.processedInputMovement.x * _movementVars.movementSpeed;
        float speedDifX = targetSpeedX - rb.velocity.x;
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? _movementVars.acceleration : _movementVars.decceleration;
        float movementX = Mathf.Pow(Mathf.Abs(speedDifX) * accelRateX, _movementVars.velocityPower) * Mathf.Sign(speedDifX);

        float targetSpeedY = _movementVars.processedInputMovement.y * _movementVars.movementSpeed;
        float speedDifY = targetSpeedY - rb.velocity.y;
        float accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? _movementVars.acceleration : _movementVars.decceleration;
        float movementY = Mathf.Pow(Mathf.Abs(speedDifY) * accelRateY, _movementVars.velocityPower) * Mathf.Sign(speedDifY);

        rb.AddForce(movementX * Vector2.right);
        rb.AddForce(movementY * Vector2.up);
    }

    private void RotationMath()
    {
        if (_movementVars.processedInputMovement.x > 0f)
        {
            ghostSlime_rotation -= ghostSlime_rotationSpeed;
        }

        if (_movementVars.processedInputMovement.x < 0f)
        {
            ghostSlime_rotation += ghostSlime_rotationSpeed;
        }   

        if (_movementVars.processedInputMovement.x == 0f && ghostSlime_rotation != 0f)
        {
            ghostSlime_rotation = Mathf.Lerp(ghostSlime_rotation, 0f, Time.deltaTime * ghostSlime_rotationReturnSpeed);
        }

        ghostSlime_rotation = Mathf.Clamp(ghostSlime_rotation, -16f, 16f);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, ghostSlime_rotation));
    }

    private void FixedUpdate()
    {
        MovementInputProcessor(); // Processes Raw Input to Processed Input

        RotationMath(); // Tilts the Ghost Slime overtime based on movement

        DecellerationStallUpdate(); // Updates Decelleration Stall clock

        MovementStallUpdate(); // Updates Movement Stall clock

        MainMovementMath(); // Main movement math
    }
}
