using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSlime_MovementVariables : MonoBehaviour
{
    public float initialDecceleration;
    public float movementStallTime;
    public bool isMovementStalled;

    [Header("Velocity")]
    public float acceleration;
    public float decceleration;
    public float velocityPower;
    public float movementSpeed;
    public float deccelerationTimer;

    [Header("General Movement")]
    public Vector2 rawInputMovement;
    public Vector2 processedInputMovement;
}
