using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private Transform originPoint;
    [SerializeField] private SpriteRenderer sr_door;
    [SerializeField] private Sprite openDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                OpenDoor(collision.transform);
                gameObject.SetActive(false);
            }
        }
    }

    private void OpenDoor(Transform playerPos)
    {
        sr_door.sprite = openDoor;

        if (originPoint.position.x < playerPos.position.x)
        {
            sr_door.flipX = true;
        }
    }
}
