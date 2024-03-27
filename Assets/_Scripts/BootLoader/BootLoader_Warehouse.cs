using Cinemachine;
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

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_overlayLoadingScreen;
    [SerializeField] private string scene_bootloaderDevTools;
    [SerializeField] private string scene_bootloaderGlobal;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_bootloaderDevTools, true);
        _sceneQueue.LoadScene(scene_bootloaderGlobal, true);
        _sceneQueue.LoadScene(scene_overlayLoadingScreen, true);
        StartCoroutine(DelayedAwake());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();

        if (_checkpoint._checkpointQueue.checkpointId == "" || _checkpoint._checkpointQueue.checkpointId == "0")
        {
            StartCoroutine(_warehouseIntro.InitiateWarehouseIntro());
        } else
        {
            _warehouseIntro.AbortWarehouseIntro();
            _checkpoint.InitiateCheckpointHandling();
        }

        Debug.Log("Checkpoint ID: " + _checkpoint._checkpointQueue.checkpointId);
    }
}