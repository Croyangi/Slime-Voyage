using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_StateMachineHelper : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] public GameObject baseSlime;
    [SerializeField] public Vector2 colliderBounds;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public BoxCollider2D col_slime;

    [Header("Building Block References")]
    [SerializeField] public BaseSlime_MovementVariables _movementVars;

    [Header("Collider References")]
    public Collider2D col_isGrounded;
    public Collider2D col_touchingLeft;
    public Collider2D col_touchingRight;
    public Collider2D col_onEdgeLeft;
    public Collider2D col_onEdgeRight;

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

    private void Awake()
    {
        //colliderBounds = baseSlime.GetComponent<Collider2D>().bounds.extents;
        facingDirection = 1;
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

        if (_movementVars.processedInputMovement.x == 1)
        {
            facingDirection = 1;
        } else if (_movementVars.processedInputMovement.x == -1)
        {
            facingDirection = -1;
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
                if ((_tags.CheckTags(tag_isSolidGround.name) == true || _tags.CheckTags(tag_isPlatform.name) == true) && rb.velocity.y > -5 && rb.velocity.y < 5)
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
