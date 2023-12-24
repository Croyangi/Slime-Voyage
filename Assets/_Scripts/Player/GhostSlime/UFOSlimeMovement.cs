using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UFOSlimeMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PlayerInput playerInput = null;
    //[SerializeField] private PlayerStateScriptObj playerStateScriptObj;
    [SerializeField] private PlayerMovementScriptObj playerMovementScriptObj;
    [SerializeField] private GameObject ufoBeamAnchor;

    [Header("Variables")]
    [SerializeField] public Vector2 spriteBounds;
    [SerializeField] public Vector2 colliderBounds;
    [SerializeField] public float rotateZ;
    [SerializeField] private GameObject ufoBeamObject;
    [SerializeField] private Vector2 ufoBeamObjectOffset;
    [SerializeField] private bool isUFOBeamTraveling;
    [SerializeField] private bool isUFOBeamCaptured;
    [SerializeField] private float ufoBeamMaxCaptureDistance;
    [SerializeField] private float ufoBeamStrength;
    [SerializeField] private float ufoBeamMaxHoldDistance;

    private void Awake()
    {
        rotateZ = transform.localRotation.eulerAngles.z;

        spriteBounds = sr.bounds.size;

        playerInput = new PlayerInput();
        spriteBounds = GetComponent<SpriteRenderer>().bounds.size;

        playerMovementScriptObj.ufoSlime.rawInputMovement.x = 0f;
        playerMovementScriptObj.ufoSlime.rawInputMovement.y = 0f;
    }

    private void OnEnable()
    {
        playerInput.GhostSlime.Movement.performed += OnMovementPerformed;
        playerInput.GhostSlime.Movement.canceled += OnMovementCancelled;
        playerInput.GhostSlime.Beam.performed += OnBeamPerformed;
        playerInput.GhostSlime.Beam.canceled += OnBeamCancelled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.GhostSlime.Movement.performed -= OnMovementPerformed;
        playerInput.GhostSlime.Movement.canceled -= OnMovementCancelled;
        playerInput.GhostSlime.Beam.performed -= OnBeamPerformed;
        playerInput.GhostSlime.Beam.canceled -= OnBeamCancelled;
        playerInput.Disable();
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        playerMovementScriptObj.ufoSlime.rawInputMovement = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        playerMovementScriptObj.ufoSlime.rawInputMovement.x = 0f;
        playerMovementScriptObj.ufoSlime.rawInputMovement.y = 0f;
    }

    private void OnBeamPerformed(InputAction.CallbackContext value)
    {
        if (isUFOBeamCaptured)
        {
            isUFOBeamCaptured = false;
        } else
        {
            isUFOBeamTraveling = true;
        }
    }

    private void OnBeamCancelled(InputAction.CallbackContext value)
    {
    }

    private void FlipSprite()
    {
        if (!playerMovementScriptObj.ufoSlime.isSpriteFlipped)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }

    private void MovementStall()
    {
        playerMovementScriptObj.ufoSlime.processedInputMovement.x = 0;
        playerMovementScriptObj.ufoSlime.processedInputMovement.y = 0;
        playerMovementScriptObj.movementStallTime -= Time.deltaTime;
    }

    private void FreezePlayerMovement()
    {
        playerMovementScriptObj.ufoSlime.processedInputMovement.x = 0;
        rb.velocity = Vector3.zero;
    }

    private void MainMovementMath()
    {
        float targetSpeedX = playerMovementScriptObj.ufoSlime.processedInputMovement.x * playerMovementScriptObj.ufoSlime.movementSpeed;
        float speedDifX = targetSpeedX - rb.velocity.x;
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? playerMovementScriptObj.ufoSlime.acceleration : playerMovementScriptObj.ufoSlime.decceleration;
        float movementX = Mathf.Pow(Mathf.Abs(speedDifX) * accelRateX, playerMovementScriptObj.ufoSlime.velocityPower) * Mathf.Sign(speedDifX);

        float targetSpeedY = playerMovementScriptObj.ufoSlime.processedInputMovement.y * playerMovementScriptObj.ufoSlime.movementSpeed;
        float speedDifY = targetSpeedY - rb.velocity.y;
        float accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? playerMovementScriptObj.ufoSlime.acceleration : playerMovementScriptObj.ufoSlime.decceleration;
        float movementY = Mathf.Pow(Mathf.Abs(speedDifY) * accelRateY, playerMovementScriptObj.ufoSlime.velocityPower) * Mathf.Sign(speedDifY);

        rb.AddForce(movementX * Vector2.right);
        rb.AddForce(movementY * Vector2.up);
    }

    private void GetObjectFromUFOBeam()
    {
        Tags _tags;
        RaycastHit2D hit = Physics2D.Raycast(ufoBeamAnchor.transform.position, Vector2.down, ufoBeamMaxCaptureDistance);

        if (hit.collider != null) // On Hit
        {
            isUFOBeamTraveling = false;

            GameObject collidedObject = hit.collider.gameObject;
            if (collidedObject.GetComponent<Tags>() != null)
            {
                _tags = collidedObject.GetComponent<Tags>();
                if (_tags.CheckTags("IsUFOBeamInteractable") == true)
                {
                    //Debug.Log("hit something!" + hit.collider.gameObject);
                    ufoBeamObject = hit.collider.gameObject;
                    ufoBeamObjectOffset = ufoBeamObject.transform.position - gameObject.transform.position;
                    isUFOBeamTraveling = false;
                    isUFOBeamCaptured = true;

                } else
                {
                    //Debug.Log("got nothing..." + hit.collider.gameObject);
                }
            }
        }
    }

    private void UFOBeamMovement()
    {
        if (ufoBeamObject != null)
        {
            Vector2 targetPosition = new Vector2(transform.position.x + ufoBeamObjectOffset.x, transform.position.y + ufoBeamObjectOffset.y);

            // Calculate the desired velocity towards the target position
            Vector2 desiredVelocity = (targetPosition - (Vector2) ufoBeamObject.transform.position) / Time.deltaTime;

            // Smoothly adjust the velocity
            ufoBeamObject.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(ufoBeamObject.GetComponent<Rigidbody2D>().velocity, desiredVelocity, ufoBeamStrength * Time.deltaTime);

            // Clamping rotation, no crazy spins here
            ufoBeamObject.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(ufoBeamObject.GetComponent<Rigidbody2D>().angularVelocity, -30f, 30f);

            if (GetDistanceBetweenTwoPoints(ufoBeamObject.transform.position, ufoBeamAnchor.transform.position) > ufoBeamMaxHoldDistance)
            {
                isUFOBeamCaptured = false;
            }
        }
    }

    private float GetDistanceBetweenTwoPoints(Vector2 pointA, Vector2 pointB)
    {
        float distance;

        float group1 = Mathf.Pow((pointB.x - pointA.x), 2);
        float group2 = Mathf.Pow((pointB.y - pointA.y), 2);
        distance = Mathf.Sqrt(group1 + group2);

        return distance;
    }

    private void RotationMath()
    {
        float rotatingSpeed = 1f;

        if (playerMovementScriptObj.ufoSlime.processedInputMovement.x > 0f)
        {
            rotateZ -= rotatingSpeed;
        } else if (playerMovementScriptObj.ufoSlime.processedInputMovement.x < 0f) 
        {
            rotateZ += rotatingSpeed;
        } else if (playerMovementScriptObj.ufoSlime.processedInputMovement.x == 0f && rotateZ > 0) 
        {
            rotateZ -= rotatingSpeed;
        } else if (playerMovementScriptObj.ufoSlime.processedInputMovement.x == 0f && rotateZ < 0)
        {
            rotateZ += rotatingSpeed;
        }

        rotateZ = Mathf.Clamp(rotateZ, -16f, 16f);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));


    }

    private void FixedUpdate()
    {
        RotationMath();

        playerMovementScriptObj.ufoSlime.processedInputMovement.x = playerMovementScriptObj.ufoSlime.rawInputMovement.x; // Input Movement is raw data
        playerMovementScriptObj.ufoSlime.processedInputMovement.y = playerMovementScriptObj.ufoSlime.rawInputMovement.y;

        if (playerMovementScriptObj.ufoSlime.processedInputMovement.x < 0)
        {
            playerMovementScriptObj.ufoSlime.isSpriteFlipped = true;
        } // Flips sprite based on player's input
        else if (playerMovementScriptObj.ufoSlime.processedInputMovement.x > 0)
        {
            playerMovementScriptObj.ufoSlime.isSpriteFlipped = false;
        }

        if (rb.velocity.x > 0.1)
        {
            playerMovementScriptObj.ufoSlime.isSpriteFlipped = false;
        } // Flips sprite based on current X velocity
        else if (rb.velocity.x < -0.1)
        {
            playerMovementScriptObj.ufoSlime.isSpriteFlipped = true;
        }

        if (playerMovementScriptObj.movementStallTime > 0) { MovementStall(); } // Sets movement stall

        // Flips sprite
        FlipSprite(); // Checks current isFlippedSprite variable to... flip the sprite

        //if (!playerStateScriptObj.canPlayerMove) { FreezePlayerMovement(); } // Freezes player movement

        MainMovementMath(); // Main movement math

        if (isUFOBeamTraveling) { GetObjectFromUFOBeam(); }
        if (isUFOBeamCaptured) { UFOBeamMovement(); }
    }
}
