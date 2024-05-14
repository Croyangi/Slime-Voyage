using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private bool isReached;
    [SerializeField] private GameObject lightTip;
    [SerializeField] private AudioClip audioClip_checkpointLightOn;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [SerializeField] private ScriptObj_AreaId _areaId;
    [SerializeField] private string id;

    [Header("Animation Clips")]
    const string CHECKPOINT_ON = "Checkpoint_On";
    const string CHECKPOINT_OFF = "Checkpoint_Off";

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }


    public void LoadData(GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> checkpointsReached = areaSet.checkpointsReached;

        checkpointsReached.TryGetValue(id, out isReached);
        if (isReached)
        {
            ReachedCheckpoint();
        }
    }

    public void SaveData(ref GameData data)
    {
        // Search up area data based on id
        AreaSet areaSet = data.SearchAreaWithId(_areaId.name);
        SerializableDictionary<string, bool> checkpointsReached = areaSet.checkpointsReached;

        if (checkpointsReached.ContainsKey(id))
        {
            checkpointsReached.Remove(id);
        }
        checkpointsReached.Add(id, isReached);
    }

    private void Awake()
    {
        if (isReached)
        {
            _animator.Play(CHECKPOINT_ON);
        } else
        {
            _animator.Play(CHECKPOINT_OFF);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && isReached == false)
            {
                ReachedCheckpoint();
                Manager_SFXPlayer.instance.PlaySFXClip(audioClip_checkpointLightOn, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f, 1f, 1f, 30f);
                DataPersistenceManager.instance.SaveGame();
            }
        }
    }

    private void ReachedCheckpoint()
    {
        lightTip.SetActive(true);
        _animator.Play(CHECKPOINT_ON);
        isReached = true;
    }


}
