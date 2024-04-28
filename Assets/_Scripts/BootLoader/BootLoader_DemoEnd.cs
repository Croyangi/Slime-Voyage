using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_DemoEnd : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    private void Awake()
    {
        if (Manager_LoadingScreen.instance == null)
        {
            _sceneQueue.LoadScene(scene_loadingScreen, true);
        }
        StartCoroutine(DelayedAwake());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }
}
