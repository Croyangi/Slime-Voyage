using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private Tags _tags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.GetComponent<Tags>() != null)
        {
            _tags = collidedObject.GetComponent<Tags>();
            if (_tags.CheckTags("Player") == true)
            {
                Manager_PlayerState.instance.InitiatePlayerDeath();
            }
        }
    }
}
