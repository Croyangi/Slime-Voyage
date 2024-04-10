using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Handler_Checkpoint : MonoBehaviour
{
    [SerializeField] public ScriptObj_CheckpointQueue _checkpointQueue;
    [SerializeField] private Handler_ChunkshipCutscene _chunkshipCutscene;
    [SerializeField] private string[] ids;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private AudioClip audioClip_breakingProtocol;

    public void InitiateCheckpointHandling()
    {
        if (_checkpointQueue.checkpointId == "dev")
        {
            Debug.Log("Dev Checkpoint Enabled");
            return;
        }

        if (_checkpointQueue.checkpointId == "WarehouseCheckpoint0")
        {
            InitiateWarehouseBeginningCheckpoint();
            return;
        }

        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == _checkpointQueue.checkpointId)
            {
                Debug.Log("HELPPPP WE FOUND A CHECKPOINTTTT");
                _chunkshipCutscene.InitiateCheckpointCutscene(checkpoints[i]);
            }
        }
    }

    private void InitiateWarehouseBeginningCheckpoint()
    {
        GameObject baseSlime = Manager_PlayerState.instance.player;
        baseSlime.transform.position = checkpoints[0].transform.position;
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_breakingProtocol, transform, 0.5f, true, Manager_AudioMixer.instance.mixer_music);
    }
}
