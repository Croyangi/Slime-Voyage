using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite activeCheckpoint;
    [SerializeField] private SpriteRenderer sr;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                sr.sprite = activeCheckpoint;
            }
        }
    }


}
