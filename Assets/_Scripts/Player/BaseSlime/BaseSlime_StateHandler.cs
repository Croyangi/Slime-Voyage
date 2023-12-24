using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_StateHandler : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject baseSlime;
    [SerializeField] private Vector2 colliderBounds;

    [Header("Building Block References")]
    [SerializeField] private BaseSlime_MovementVariables _movementVars;

    [Header("Collider References")]
    [SerializeField] private Collider2D _isGroundedCollider;
    [SerializeField] private Collider2D _touchingColliderLeft;
    [SerializeField] private Collider2D _touchingColliderRight;
    [SerializeField] private Collider2D _onEdgeColliderLeft;
    [SerializeField] private Collider2D _onEdgeColliderRight;

    [Header("Tags")]
    const string IS_SOLID_GROUND = "IsSolidGround";
    const string IS_PLATFORM = "IsPlatform";
    const string IS_STICKABLE = "IsStickable";

    [Header("Base Slime States")]
    public bool isGrounded;
    public int isOnEdge;
    public Vector2 touchingDirection;
    public Vector2 stickingDirection;
    public bool isPermanentlySticking; // Disables after touching ground again

    private void Awake()
    {
        colliderBounds = baseSlime.GetComponent<BoxCollider2D>().bounds.extents;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGroundedUpdate();

        if (IsGroundedUpdate()) { isPermanentlySticking = false; }

        isOnEdge = OnEdgeUpdate();

        stickingDirection = StickingDirectionUpdate();

        PermanentlyStickingUpdate();

        touchingDirection = TouchingDirectionUpdate();
    }

    private bool IsGroundedUpdate()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_isGroundedCollider, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_SOLID_GROUND) == true || _tags.CheckTags(IS_PLATFORM) == true)
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
        Physics2D.OverlapCollider(_touchingColliderLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        //Vector2 boxSize = new Vector2(0.15f, colliderBounds.y);
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - colliderBounds.x, transform.position.y), boxSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_STICKABLE) == true)
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
        Physics2D.OverlapCollider(_touchingColliderRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_STICKABLE) == true)
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
        } else if (touchingDirection.x == -1 && isPermanentlySticking)
        {
            stickingDirection.x = -1;
        }
    }

    private bool IsTouchingLeft()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_touchingColliderLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_SOLID_GROUND) == true)
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
        Physics2D.OverlapCollider(_touchingColliderRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_SOLID_GROUND) == true)
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
        Physics2D.OverlapCollider(_onEdgeColliderLeft, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_SOLID_GROUND) == true || _tags.CheckTags(IS_PLATFORM) == true)
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
        Physics2D.OverlapCollider(_onEdgeColliderRight, new ContactFilter2D(), colliders);

        Tags _tags;

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            if (temp.GetComponent<Tags>() != null)
            {
                _tags = temp.GetComponent<Tags>();
                if (_tags.CheckTags(IS_SOLID_GROUND) == true || _tags.CheckTags(IS_PLATFORM) == true)
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
