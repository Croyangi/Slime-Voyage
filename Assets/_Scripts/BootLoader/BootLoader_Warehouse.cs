using Cinemachine;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Handler_WarehouseIntro _warehouseIntro;
    [SerializeField] private Handler_Checkpoint _checkpoint;
    [SerializeField] private Handler_WarehouseSwapeeMode _swapeeMode;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_bootloaderDevTools;
    [SerializeField] private string scene_bootloaderGlobal;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_bootloaderDevTools, true);
        _sceneQueue.LoadScene(scene_bootloaderGlobal, true);

        StartCoroutine(DelayedAwake());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();

        if (_checkpoint._checkpointQueue.checkpointId == "" || _checkpoint._checkpointQueue.checkpointId == "0")
        {
            StartCoroutine(_warehouseIntro.InitiateWarehouseIntro());
        } else if (_checkpoint._checkpointQueue.checkpointId == "SwapeeMode")
        {
            _swapeeMode.EnableSwapeeMode();
        } else
        {
            _checkpoint.InitiateCheckpointHandling();
        }

        if (Manager_LoadingScreen.instance != null)
        {
            Manager_PauseMenu.instance.isUnpausable = true;
            Manager_LoadingScreen.instance.OpenLoadingScreen();
            yield return new WaitForSeconds(3f);
            Manager_PauseMenu.instance.isUnpausable = false;
        }

        Debug.Log("Checkpoint ID: " + _checkpoint._checkpointQueue.checkpointId);
    }
}