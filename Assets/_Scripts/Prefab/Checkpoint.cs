using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private Sprite activeCheckpoint;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private bool isReached;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [SerializeField] private string id;

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }


    public void LoadData(GameData data)
    {
        data.checkpointsReached.TryGetValue(id, out isReached);
        if (isReached)
        {
            ReachedCheckpoint();
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.checkpointsReached.ContainsKey(id))
        {
            data.checkpointsReached.Remove(id);
        }
        data.checkpointsReached.Add(id, isReached);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && isReached == false)
            {
                ReachedCheckpoint();
                DataPersistenceManager.instance.SaveGame();
            }
        }
    }

    private void ReachedCheckpoint()
    {
        sr.sprite = activeCheckpoint;
        isReached = true;
    }


}
