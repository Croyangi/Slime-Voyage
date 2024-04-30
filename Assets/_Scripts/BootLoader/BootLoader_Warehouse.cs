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
    [SerializeField] private ScriptObj_ModifierMode _modifierMode;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_bootloaderDevTools;
    [SerializeField] private string scene_bootloaderGlobal;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_bootloaderDevTools, true);
        //_sceneQueue.LoadScene(scene_bootloaderGlobal, true);

        StartCoroutine(DelayedAwake());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();

        if (_checkpoint._checkpointQueue.checkpointId == "WarehouseIntro")
        {
            Debug.Log("WAREHOUSE INTRO");
            StartCoroutine(_warehouseIntro.InitiateWarehouseIntro());
        } else
        {
            _checkpoint.InitiateCheckpointHandling();
        }

        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Finished");
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }

        Debug.Log("Checkpoint ID: " + _checkpoint._checkpointQueue.checkpointId);
    }
}