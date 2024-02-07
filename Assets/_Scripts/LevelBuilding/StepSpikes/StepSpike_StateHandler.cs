using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_StateHandler : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject stepSpike_parentObject;

    [Header("Step Spike States")]
    public bool isGrounded;
    public bool isTouchingLeft;
    public bool isTouchingRight;

    [Header("Collider References")]
    [SerializeField] private Collider2D _isGroundedCollider;
    [SerializeField] private Collider2D _touchingLeftCollider;
    [SerializeField] private Collider2D _touchingRightCollider;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _isSolidGroundTag;
    [SerializeField] private TagsScriptObj _isPlatformTag;

    [SerializeField] private ContactFilter2D _ignoreStepSpikeFilter;

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        isTouchingLeft = IsTouchingLeft();
        isTouchingRight = IsTouchingRight();

    }

    private bool IsGrounded()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_isGroundedCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;

            float angle = stepSpike_parentObject.transform.rotation.eulerAngles.z;
            if (angle > 180) { angle -= 360; } // Convert to the range -180 to 180 degrees

            if (collider.gameObject.TryGetComponent<Tags>(out var _tags) && temp != stepSpike_parentObject && Mathf.Abs(angle) < 15f)
            {
                if (_tags.CheckTags(_isSolidGroundTag.name) == true || _tags.CheckTags(_isPlatformTag.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsTouchingLeft()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_touchingLeftCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags) && temp != stepSpike_parentObject)
            {
                if (_tags.CheckTags(_isSolidGroundTag.name) == true || _tags.CheckTags(_isPlatformTag.name) == true)
                {
                    return true;
                }

                if (temp.GetComponent<Rigidbody2D>() != null)
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
        Physics2D.OverlapCollider(_touchingRightCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            GameObject temp = collider.gameObject;
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags) && temp != stepSpike_parentObject)
            {
                if (_tags.CheckTags(_isSolidGroundTag.name) == true || _tags.CheckTags(_isPlatformTag.name) == true)
                {
                    return true;
                }

                if (temp.GetComponent<Rigidbody2D>() != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
