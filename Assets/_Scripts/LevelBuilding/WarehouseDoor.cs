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
    [SerializeField] private Collider2D col_lockedDoor;

    [Header("Settings")]
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isUnlockableLeft;
    [SerializeField] private bool isUnlockableRight;

    private void Awake()
    {
        if (isLocked)
        {
            col_lockedDoor.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                OpenDoor(collision.transform);
            }
        }
    }

    private void OpenDoor(Transform playerPos)
    {
        if (originPoint.position.x < playerPos.position.x && isUnlockableRight)
        {
            isLocked = false;
        } else if (originPoint.position.x > playerPos.position.x && isUnlockableLeft)
        {
            isLocked = false;
        }

        if (isLocked == false)
        {
            sr_door.sprite = openDoor;

            if (originPoint.position.x < playerPos.position.x)
            {
                sr_door.flipX = true;
            }

            gameObject.SetActive(false);
        }
    }
}
