using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKiller : MonoBehaviour
{
    [SerializeField] private TagsScriptObj tag_isTestObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_isTestObject.name) == true)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
