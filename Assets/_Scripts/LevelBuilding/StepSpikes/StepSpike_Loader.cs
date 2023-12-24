using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Loader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject loadedObjects;
    [SerializeField] private Collider2D _checkExistingObjectsCollider;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj[] _tagsToCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            foreach (TagsScriptObj tag in _tagsToCheck)
            {
                if (_tags.CheckTags(tag.name) == true)
                {
                    LoadObjects();
                    return;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            foreach (TagsScriptObj tag in _tagsToCheck)
            {
                if (_tags.CheckTags(tag.name) == true && !CheckExistingObjects())
                {
                    DeloadObjects();
                    return;
                }
            }
        }
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_checkExistingObjectsCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                foreach (TagsScriptObj tag in _tagsToCheck)
                {
                    if (_tags.CheckTags(tag.name) == true)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void LoadObjects()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        loadedObjects.SetActive(true);
    }

    private void DeloadObjects()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        loadedObjects.SetActive(false);
    }
}
