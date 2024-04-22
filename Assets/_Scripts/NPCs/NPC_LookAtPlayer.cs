using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_LookAtPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private bool isDetected;
    [SerializeField] private bool isInverseLooking;
    [SerializeField] private Collider2D col_detect;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                isDetected = true;
                StartCoroutine(LookAtPlayer(collision.gameObject.transform));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true && CheckExistingObjects() == false)
            {
                isDetected = false;
            }
        }
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_detect, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(_playerTag.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator LookAtPlayer(Transform objectToLookAt)
    {
        // Flips sprite based on object location
        if (gameObject.transform.position.x < objectToLookAt.position.x)
        {
            sr.flipX = false;
        } else
        {
            sr.flipX = true;
        }

        if (isInverseLooking)
        {
            sr.flipX = !sr.flipX;
        }

        if (isDetected)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(LookAtPlayer(objectToLookAt));
        }

    }
}
