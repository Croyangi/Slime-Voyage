using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader_MovementGym : MonoBehaviour
{
    [Header("References")]

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_bootloaderDevTools;
    [SerializeField] private string scene_bootloaderGlobal;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_bootloaderDevTools, true);
        _sceneQueue.LoadScene(scene_bootloaderGlobal, true);
        //_sceneQueue.LoadScene(scene_loadingScreen, true);
        //StartCoroutine(DelayedAwake());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
    }

}
