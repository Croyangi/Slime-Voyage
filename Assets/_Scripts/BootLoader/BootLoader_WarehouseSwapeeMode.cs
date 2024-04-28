using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_WarehouseSwapeeMode : MonoBehaviour
{
    [Header("References")]

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_bootloaderDevTools;
    [SerializeField] private string scene_bootloaderGlobal;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_bootloaderDevTools, true);

        StartCoroutine(DelayedAwake());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    private IEnumerator DelayedAwake()
    {
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
    }
}
