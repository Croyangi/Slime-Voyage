using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSpike_Detect : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tags _tags;
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.GetComponent<Tags>() != null)
        {
            _tags = collidedObject.GetComponent<Tags>();
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                Manager_PlayerState.instance.InitiatePlayerDeath();
            }
        }
    }

}
