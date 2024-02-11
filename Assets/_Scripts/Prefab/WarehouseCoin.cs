using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseCoin : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tags _tags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tags.CheckGameObjectTags(collision.gameObject, "Player") == true)
        {
            Destroy(gameObject);
        }

    }
}
