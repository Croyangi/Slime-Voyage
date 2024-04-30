using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Manager_LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject flavorGraphics;
    [SerializeField] private GameObject chunkfishDisc;
    [SerializeField] private ScriptObj_FlavorText _flavorText;
    [SerializeField] private TextMeshProUGUI tmp_flavorText;

    [SerializeField] private GameObject mainCamera;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;

    public static Manager_LoadingScreen instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Loading Screen Manager in the scene.");
        }
        instance = this;

        GenerateRandomFlavorText();
        RotateChunkfishDisc();
    }

    [ContextMenu("Generate Random Flavor Text")]
    private void GenerateRandomFlavorText()
    {
        int random = Random.Range(0, _flavorText.flavorText.Count);
        tmp_flavorText.text = _flavorText.flavorText[random];
    }

    private void RotateChunkfishDisc()
    {
        LeanTween.rotateZ(chunkfishDisc, 0, 0).setIgnoreTimeScale(true);
        LeanTween.rotateAroundLocal(chunkfishDisc, Vector3.forward, -360, 4f).setLoopClamp().setIgnoreTimeScale(true).setDelay(0.1f);
    }

    [ContextMenu("Close Loading Screen")]
    public void OpenLoadingScreen()
    {
        PrepareOpenLoadingScreen();
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 1f).setEaseInOutBack().setDelay(0.5f).setIgnoreTimeScale(true); ;
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500f, 1.2f).setEaseInQuart().setDelay(1f).setIgnoreTimeScale(true); ;
    }

    [ContextMenu("Prepare Close Loading Screen")]
    public void PrepareOpenLoadingScreen()
    {
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 0).setIgnoreTimeScale(true);
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0, 0).setIgnoreTimeScale(true); ;
    }

    [ContextMenu("Open Loading Screen")]
    public void CloseLoadingScreen()
    {
        PrepareCloseLoadingScreen();
        GenerateRandomFlavorText();
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0f, 1.2f).setEaseInQuart().setDelay(0.5f).setIgnoreTimeScale(true);
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 1f).setEaseInOutBack().setDelay(1.3f).setIgnoreTimeScale(true); ;
    }

    [ContextMenu("Prepare Open Loading Screen")]
    public void PrepareCloseLoadingScreen()
    {
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500, 0).setIgnoreTimeScale(true); ;
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 0).setIgnoreTimeScale(true); ;
    }

    public void InitiateLoadSceneTransfer(string loadedScene, string unloadedSceneName)
    {
        StartCoroutine(OnLoadSceneTransfer(loadedScene, unloadedSceneName));
    }

    public IEnumerator OnLoadSceneTransfer(string loadedScene, string unloadedSceneName)
    {
        Debug.Log("Transfering Scene");

        CloseLoadingScreen();
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.UnloadSceneAsync(unloadedSceneName);
        mainCamera.SetActive(true);

       // _sceneQueue.UnqueueAllScenes();
        //_sceneQueue.QueueScene(loadedScene, true);

        StartCoroutine(ProcessLoadSceneTransfer(loadedScene));
    }

    private IEnumerator ProcessLoadSceneTransfer(string loadedScene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadedScene, LoadSceneMode.Additive);

        // Wait until the asynchronous scene loading is complete
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded: " + loadedScene);
        mainCamera.SetActive(false);
    }
}
