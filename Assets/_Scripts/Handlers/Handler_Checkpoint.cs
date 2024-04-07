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

    public void InitiateCheckpointHandling()
    {
        if (_checkpointQueue.checkpointId == "dev")
        {
            Debug.Log("Dev Checkpoint Enabled");
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
}
