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
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == _checkpointQueue.checkpointId)
            {
                if (_checkpointQueue.checkpointId == null)
                {
                    Debug.Log("HELPPPP ITS NULLLL");
                }
                Debug.Log("HELPPPP WE FOUND A CHECKPOINTTTT");
                _chunkshipCutscene.InitiateCheckpointCutscene(checkpoints[i]);
            }
        }
    }
}
