using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_MovementVariables : MonoBehaviour
{
    public float initialDeceleration;
    public float movementStallTime;
    public bool isMovementStalled;

    [Header("Coyote Time")]
    public float coyoteTime;
    public float coyoteJumpTimer;

    [Header("Jump Buffer Time")]
    public float jumpBuffer;
    public float jumpBufferTimer;

    [Header("Velocity")]
    public float acceleration;
    public float groundedDeceleration;
    public float exceedDeceleration;
    public float velocityPower;
    public float movementSpeed;
    public float deccelerationTimer;

    public float maxFallSpeed;

    [Header("General Movement")]
    public float jumpStrength;
    public Vector2 rawInputMovement;
    public Vector2 processedInputMovement;

    [Header("Wall Jump")]
    public float wallJumpStrengthHorizontal;
    public float wallJumpStrengthVerticle;
    public float wallJumpStallTime;

    public float stickingWallSpeed;
    public float stickingWallAcceleration;
    public float stickingWallDeceleration;
    public float stickingWallVelocityPower;
}