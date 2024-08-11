using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_MovementGym : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;
    [SerializeField] private ScriptObj_SceneName scene_activeScene;
    [SerializeField] private ScriptObj_SceneName scene_devTools;

    private void Awake()
    {
        if (Manager_LoadingScreen.instance == null)
        {
            _sceneQueue.LoadScene(scene_loadingScreen.name, true);
        }

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
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }

}
