using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_CheckpointLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] private bool isClicked = false;
    [SerializeField] private string[] ids;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private ScriptObj_CheckpointQueue _checkpointQueue;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_theWarehouse;
    [SerializeField] private string scene_overlayLoadingScreen;

    private void Start()
    {
        // Reset
        _checkpointQueue.checkpointId = "";
        Debug.Log("RESSSETTTING");
        Debug.Log("Checkpoint ID: " + _checkpointQueue.checkpointId);
    }

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

    public void OnCheckpointButtonClicked(int id)
    {
        if (isClicked == false)
        {
            isClicked = true;
            _checkpointQueue.checkpointId = ids[id];
            Debug.Log(_checkpointQueue.checkpointId);

            StartCoroutine(LoadTheWarehouse());
        }

    }

    private IEnumerator LoadTheWarehouse()
    {
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(3);
        Manager_LoadingScreen.instance.LoadTrueLoadingScreen(scene_theWarehouse);
    }
}
