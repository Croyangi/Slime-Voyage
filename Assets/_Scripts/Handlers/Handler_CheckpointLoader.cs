using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_CheckpointLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string[] ids;
    [SerializeField] private GameObject[] checkpoints;

    public void LoadData(GameData data)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (data.checkpointsReached.TryGetValue(ids[i], out bool isReached) && isReached == true)
            {
                checkpoints[i].SetActive(true);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
    }
}
