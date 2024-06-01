using Cinemachine;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private Handler_WarehouseIntro _warehouseIntro;
    [SerializeField] private Handler_WarehouseCheckpoint _checkpoint;
    [SerializeField] private ScriptObj_ModifierMode _modifierMode;

    [SerializeField] private ScriptObj_AreaId _areaId;
    [SerializeField] private bool isCompleted;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_devTools;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;
    [SerializeField] private ScriptObj_SceneName scene_activeScene;
    [SerializeField] private ScriptObj_SceneName scene_loadedScene;
    [SerializeField] private ScriptObj_SceneName scene_deloadedScene;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_devTools.name, true);

        StartCoroutine(DelayedAwake());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene.name));
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
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen.name, LoadSceneMode.Additive);

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

    public void OnWarehouseComplete()
    {
        isCompleted = true;
        DataPersistenceManager.instance.SaveGame();
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_loadedScene.name, scene_deloadedScene.name);
    }

    public void LoadData(GameData data)
    {
        data.areasCompleted.TryGetValue(_areaId.name, out isCompleted);
    }

    public void SaveData(ref GameData data)
    {
        if (data.areasCompleted.ContainsKey(_areaId.name))
        {
            data.areasCompleted.Remove(_areaId.name);
        }
        data.areasCompleted.Add(_areaId.name, isCompleted);

        if (isCompleted)
        {
            data.resultsScreenId = _areaId.name;
        }
    }
}