using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D _rigidbody2D;

    [Header("Building Blocks")]
    [SerializeField] private StepSpike_StateHandler _stateHandler;

    [Header("Variables")]
    [SerializeField] private float standUprightTimer;
    [SerializeField] private float standUprightSetTime;
    [SerializeField] public float horizontalMovement;

    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float setMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velocityPower;

    private void Awake()
    {
        if (GetRandomBool())
        {
            horizontalMovement *= -1;
            //FlipSprite();
        }
    }

    private bool GetRandomBool()
    {
        return (Random.value > 0.5f);
    }

    private void StandUprightUpdate()
    {
        standUprightTimer -= Time.deltaTime;

        if (standUprightTimer < 0)
        {
            if (_rigidbody2D.angularVelocity < 5f && _stateHandler.isGrounded == false) 
            { 
                ApplyTorque(); 
            }
            standUprightTimer = standUprightSetTime;
        }
    }

    private void ApplyTorque()
    {
        float angle = transform.eulerAngles.z;
        if (angle > 180) { angle -= 360; } // Convert to the range -180 to 180 degrees

        if (angle > 0)
        {
            _rigidbody2D.AddTorque(-3, ForceMode2D.Impulse); // Object is facing right
        }
        else if (angle < 0)
        {
            _rigidbody2D.AddTorque(3, ForceMode2D.Impulse); // Object is facing left
        }
    }

    private void ProcessMovementUpdate()
    {
        if (_stateHandler.isTouchingLeft)
        {
            horizontalMovement = 1f;
        }
        
        if (_stateHandler.isTouchingRight)
        {
            horizontalMovement = -1f;
        }
    }

    private void FixedUpdate()
    {
        ProcessMovementUpdate();

        if (!_stateHandler.isGrounded)
        {
            StandUprightUpdate();
            movementSpeed = 0;
        } else
        {
            standUprightTimer = standUprightSetTime;
            movementSpeed = setMovementSpeed;
        }

        // Main Acceleration Math
        float targetSpeed = horizontalMovement * movementSpeed;
        float speedDif = targetSpeed - _rigidbody2D.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        if (_stateHandler.isGrounded == false)
        {
            accelRate = 3f;
        }
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);
        _rigidbody2D.AddForce(movement * Vector2.right);


    }
}
