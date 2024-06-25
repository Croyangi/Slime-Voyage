using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Scriptable Object", menuName = "Cro's Scriptable Objs/New Player Movement Scriptable Obj")]
public class ScriptObj_BaseSlimeMovementVariables : ScriptableObject
{
    public float movementStallTime;

    public BaseSlime baseSlime;

    [Serializable]
    public class BaseSlime
    {
        public float initialDecceleration;

        [Header("Velocity")]
        public float acceleration;
        public float decceleration;
        public float velocityPower;
        public float movementSpeed;
        public float deccelerationTimer;

        [Header("General Movement")]
        public float jumpStrength;
        public Vector2 rawInputMovement;
        public Vector2 processedInputMovement;

        [Header("Wall Jump")]
        public float wallJumpStrengthHorizontal;
        public float wallJumpStrengthVerticle;
        public float wallJumpStallTime;

        [Header("Visuals")]
        public bool isSpriteFlipped;

        [Header("Wall Sticking")]
        public int stickingWallDirection;
        public bool isStickingWall;
        public bool isInitialStickingWall;

        public float stickingWallSpeed;
        public float stickingWallAcceleration;
        public float stickingWallDecceleration;
        public float stickingWallVelocityPower;


        [Header("Unused")]
        public float wallClingSpeed;
        public bool groundedAfterExteriorForce;
        public float jumpCancelTime;
        public bool canJumpCancel;
        public bool canJumpCancelDevDisable;
    }
}
