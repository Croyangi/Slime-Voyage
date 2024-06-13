using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseSlime_StateMachineHelper : MonoBehaviour
{

    [Header("Top Visual Speed")]
    [SerializeField] private float xVelocityPointWeight = 1f;
    [SerializeField] private float yVelocityPointWeight = 0.75f;
    [SerializeField] private float topVisualSpeedPoint = 5f;

    [Header("General References")]
    public GameObject baseSlime;
    public Vector2 colliderBounds;
    public Rigidbody2D rb;

    public float currentHighestImpactVelocityY;

    [Header("Building Block References")]
    public BaseSlime_MovementVariables _movementVars;

    [Header("Running Speed")]
    public float speedUpTimer;
    public float speedUpTimerSet;

    [Header("Collider References")]
    public BoxCollider2D col_slime;
    public BoxCollider2D col_isGrounded;
    public BoxCollider2D col_touchingLeft;
    public BoxCollider2D col_touchingRight;
    public BoxCollider2D col_onEdgeLeft;
    public BoxCollider2D col_onEdgeRight;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_isSolidGround;
    [SerializeField] private TagsScriptObj tag_isPlatform;
    [SerializeField] private TagsScriptObj tag_isStickable;

    [Header("Base Slime States")]
    public bool isGrounded;
    public int isOnEdge;
    public Vector2 touchingDirection;
    public Vector2 stickingDirection;
    public bool isPermanentlySticking; // Disables after touching ground again
    public int facingDirection;
    public bool isTopVisualSpeed;
    public bool isRunning;

    private void Awake()
    {
        //colliderBounds = baseSlime.GetComponent<Collider2D>().bounds.extents;
        facingDirection = 1;
        speedUpTimer = speedUpTimerSet;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGroundedUpdate();

        if (IsTrueGroundedUpdate()) { isPermanentlySticking = false; }
        if (_movementVars.processedInputMovement.y < 0) { isPermanentlySticking = false; }

        isOnEdge = OnEdgeUpdate();

        stickingDirection = StickingDirectionUpdate();

        PermanentlyStickingUpdate();

        touchingDirection = TouchingDirectionUpdate();

        // Facing Direction
        if (_movementVars.processedInputMovement.x == 1)
        {
            facingDirection = 1;
        } else if (_movementVars.processedInputMovement.x == -1)
        {
            facingDirection = -1;
        }

        SplatCheckUpdate(rb.velocity.y);

        TopVisualSpeedCheckUpdate();

        RunningCheckUpdate();
    }

    private void RunningCheckUpdate()
    {
        isRunning = false;

        if (_movementVars.processedInputMovement.x == 0)
        {
            // Running speed
            speedUpTimer = speedUpTimerSet;
        }

        if (Mathf.Abs(rb.velocity.x) >= _movementVars.runningSpeed || speedUpTimer <= 0)
        {
            _movementVars.movementSpeed = _movementVars.runningSpeed;
            isRunning = true;
        }
        else if (_movementVars.movementSpeed > 0) // Hotfix against compressed or lookup
        {
            _movementVars.movementSpeed = _movementVars.walkingSpeed;
        }
    }

    private void TopVisualSpeedCheckUpdate()
    {
        float combinedVelocity = Mathf.Abs(rb.velocity.x * xVelocityPointWeight) + Mathf.Abs(rb.velocity.y * yVelocityPointWeight);
        if (combinedVelocity >= topVisualSpeedPoint)
        {
            isTopVisualSpeed = true;
        } else
        {
            isTopVisualSpeed = false;
        }
    }

    private void SplatCheckUpdate(float newImpactVelocity = 0)
    {
        if (newImpactVelocity < currentHighestImpactVelocityY)
        {
            currentHighestImpactVelocityY = newImpactVelocity;
        }

        if (newImpactVelocity > 0.1f)
        {
            currentHighestImpactVelocityY = 0f;
        }
    }

    private bool IsTrueGroundedUpdate()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_isGrounded, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if ((_tags.CheckTags(tag_isSolidGround.name) == true || _tags.CheckTags(tag_isPlatform.name) == true) && rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsGroundedUpdate()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_isGrounded, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if ((_tags.CheckTags(tag_isSolidGround.name) == true || _tags.CheckTags(tag_isPlatform.name) == true) && Mathf.Abs(rb.velocity.y) < 3f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsStickingLeft()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_touchingLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        //Vector2 boxSize = new Vector2(0.15f, colliderBounds.y);
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - colliderBounds.x, transform.position.y), boxSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isStickable.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsStickingRight()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_touchingRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isStickable.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector2 StickingDirectionUpdate() // Processes sticking direction and returns as a Vector2
    {
        Vector2 direction = Vector2.zero;

        // Calls function and checks if you are holding that direction
        if (IsStickingLeft() && _movementVars.processedInputMovement.x == -1 && !isGrounded)
        {
            direction.x = -1;
            isPermanentlySticking = true;
        }

        if (IsStickingRight() && _movementVars.processedInputMovement.x == 1 && !isGrounded)
        {
            direction.x = 1;
            isPermanentlySticking = true;
        }

        return direction;
    }

    private void PermanentlyStickingUpdate()
    {
        if (touchingDirection.x == 1 && isPermanentlySticking)
        {
            stickingDirection.x = 1;
        }
        else if (touchingDirection.x == -1 && isPermanentlySticking)
        {
            stickingDirection.x = -1;
        }
    }

    private bool IsTouchingLeft()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_touchingLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isSolidGround.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsTouchingRight()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_touchingRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isSolidGround.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector2 TouchingDirectionUpdate() // Processes sticking direction and returns as a Vector2
    {
        Vector2 direction = Vector2.zero;

        // Calls function and checks if you are holding that direction
        if (IsTouchingLeft()) { direction.x = -1; }
        if (IsTouchingRight()) { direction.x = 1; }

        return direction;
    }

    private bool OnEdgeLeft()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_onEdgeLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isSolidGround.name) == true || _tags.CheckTags(tag_isPlatform.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool OnEdgeRight()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_onEdgeRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(tag_isSolidGround.name) == true || _tags.CheckTags(tag_isPlatform.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private int OnEdgeUpdate() // Processes OnEdgeDirection
    {
        int onEdge = 0;

        if (isGrounded)
        {
            if (OnEdgeLeft() && !OnEdgeRight())
            {
                onEdge = -1;
            }

            if (!OnEdgeLeft() && OnEdgeRight())
            {
                onEdge = 1;
            }
        }

        return onEdge;
    }
}
