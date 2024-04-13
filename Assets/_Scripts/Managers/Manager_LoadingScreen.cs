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
    [SerializeField] public float transitionSpeedMultiplier = 1f;

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
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 1f * transitionSpeedMultiplier).setEaseInOutBack().setDelay(1 * transitionSpeedMultiplier);
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500f, 1.2f * transitionSpeedMultiplier).setEaseInQuart().setDelay(1.5f * transitionSpeedMultiplier);
    }

    [ContextMenu("Prepare Close Loading Screen")]
    public void PrepareOpenLoadingScreen()
    {
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 0);
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0, 0);
    }

    [ContextMenu("Open Loading Screen")]
    public void CloseLoadingScreen()
    {
        PrepareCloseLoadingScreen();
        GenerateRandomFlavorText();
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0f, 1.2f * transitionSpeedMultiplier).setEaseInQuart().setDelay(1f * transitionSpeedMultiplier);
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), 0, 1f * transitionSpeedMultiplier).setEaseInOutBack().setDelay(1.8f * transitionSpeedMultiplier);
    }

    [ContextMenu("Prepare Open Loading Screen")]
    public void PrepareCloseLoadingScreen()
    {
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500, 0);
        LeanTween.moveY(flavorGraphics.GetComponent<RectTransform>(), -350, 0);
    }

    public void OnLoadSceneTransfer(string sceneName, string unloadedSceneName)
    {
        Debug.Log("hello");
        SceneManager.UnloadSceneAsync(unloadedSceneName);
        mainCamera.SetActive(true);

        _sceneQueue.UnqueueAllScenes();
        _sceneQueue.QueueScene(sceneName, true);
        StartCoroutine(ProcessLoadSceneTransfer());
    }

    private IEnumerator ProcessLoadSceneTransfer()
    {
        yield return new WaitForSeconds(3f);
        _sceneQueue.LoadQueuedScenes();
        mainCamera.SetActive(false);
    }
}
