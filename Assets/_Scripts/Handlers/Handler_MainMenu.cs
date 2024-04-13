using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_MainMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_mainMenu;
    [SerializeField] private string scene_warehouseDioramaMenu;
    [SerializeField] private string scene_loadingScreen;

    [Header("References")]
    [SerializeField] private GameObject setup;


    private void Awake()
    {
        _sceneQueue.LoadScene(scene_loadingScreen, true);
        StartCoroutine(DelayedAwake());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.PrepareCloseLoadingScreen();
    }

    public void OnMainMenuPlayButtonPressed()
    {
        StartCoroutine(LoadWarehouseDioramaMenu());
    }

    private IEnumerator LoadWarehouseDioramaMenu()
    {
        Debug.Log("pingus");
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(3);
        Manager_LoadingScreen.instance.OnLoadSceneTransfer(scene_warehouseDioramaMenu, scene_mainMenu);
    }

    public void OnMainMenuSetup()
    {
        setup.SetActive(true);
    }
}
