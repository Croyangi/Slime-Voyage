using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarehouseDoor : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private Transform originPoint;
    [SerializeField] private SpriteRenderer sr_door;
    [SerializeField] private Sprite openDoor;
    [SerializeField] private Collider2D col_lockedDoor;

    [SerializeField] private string id;
    [SerializeField] private bool isUnlocked;

    [Header("SFX")]
    [SerializeField] private AudioClip audioClip_open;
    [SerializeField] private AudioClip audioClip_locked;

    [Header("Settings")]
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isUnlockableLeft;
    [SerializeField] private bool isUnlockableRight;

    [Header("Building Blocks")]
    [SerializeField] private WarehouseDoor_DialoguePrompt _prompt;

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && _prompt.CheckExistingObjects() == false && gameObject.activeSelf == true)
            {
                _prompt.ForceQuitOutOfRange();
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
        } else if (isLocked)
        {
            _prompt.SendInteraction();
            Manager_SFXPlayer.instance.PlaySFXClip(audioClip_locked, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f);
        }

        // Unlock door
        if (isLocked == false)
        {
            UnlockDoor(playerPos);
        }
    }
    
    private void UnlockDoor(Transform playerPos = null)
    {
        sr_door.sprite = openDoor;
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_open, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f, 1f, 1f, 30f);

        // Unlock door
        if (playerPos != null)
        {
            if (originPoint.position.x < playerPos.position.x)
            {
                sr_door.flipX = true;
            }
        } else
        {
            sr_door.sprite = openDoor;

            if (isUnlockableRight)
            {
                sr_door.flipX = true;
            }
        }

        isUnlocked = true;
        gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        data.warehouseDoorsUnlocked.TryGetValue(id, out isUnlocked);
        if (isUnlocked && isLocked)
        {
            UnlockDoor();
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.warehouseDoorsUnlocked.ContainsKey(id))
        {
            data.warehouseDoorsUnlocked.Remove(id);
        }
        data.warehouseDoorsUnlocked.Add(id, isUnlocked);
    }
}
