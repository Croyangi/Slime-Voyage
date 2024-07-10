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

    [SerializeField] private bool isTransitioning;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;

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
        LeanTween.cancel(chunkfishDisc);

        LeanTween.rotateZ(chunkfishDisc, 0, 0).setIgnoreTimeScale(true);
        LeanTween.rotateAroundLocal(chunkfishDisc, Vector3.forward, -360, 4f).setLoopClamp().setIgnoreTimeScale(true).setDelay(0.1f);
    }

    [ContextMenu("Close Loading Screen")]
    public void OpenLoadingScreen()
    {
        LeanTween.cancel(blackScreen);
        LeanTween.cancel(flavorGraphics);

        PrepareOpenLoadingScreen();
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 1f).setEaseInOutBack().setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500f, 1.2f).setEaseInQuart().setDelay(0.9f).setIgnoreTimeScale(true);
    }

    [ContextMenu("Prepare Close Loading Screen")]
    public void PrepareOpenLoadingScreen()
    {
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 0).setIgnoreTimeScale(true);
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0, 0).setIgnoreTimeScale(true);
    }

    [ContextMenu("Open Loading Screen")]
    public void CloseLoadingScreen()
    {
        LeanTween.cancel(blackScreen);
        LeanTween.cancel(flavorGraphics);

        PrepareCloseLoadingScreen();
        GenerateRandomFlavorText();
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0f, 1.2f).setEaseInQuart().setDelay(0.1f).setIgnoreTimeScale(true);
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 1f).setEaseInOutBack().setDelay(0.9f).setIgnoreTimeScale(true);
    }

    [ContextMenu("Prepare Open Loading Screen")]
    public void PrepareCloseLoadingScreen()
    {
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500, 0).setIgnoreTimeScale(true);
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 0).setIgnoreTimeScale(true);
    }

    public void InitiateLoadSceneTransfer(string loadedScene)
    {
        StartCoroutine(OnLoadSceneTransfer(loadedScene));
    }

    public IEnumerator OnLoadSceneTransfer(string loadedScene)
    {
        if (isTransitioning == false)
        {
            // To prevent spam
            isTransitioning = true;
            Debug.Log("Transfering Scene");

            // VFX transition
            CloseLoadingScreen();
            yield return new WaitForSecondsRealtime(3f);

            // Deload all scenes except key ones
            Scene[] loadedScenes = GetLoadedScenes();
            foreach (Scene scene in loadedScenes)
            {
                if (scene.name != scene_loadingScreen.name)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }

            mainCamera.SetActive(true);
            StartCoroutine(ProcessLoadSceneTransfer(loadedScene));
            isTransitioning = false;
        }
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

    private Scene[] GetLoadedScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        return loadedScenes;
    }
}
