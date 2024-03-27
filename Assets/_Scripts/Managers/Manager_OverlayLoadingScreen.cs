using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager_OverlayLoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject blackScreen;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_trueLoadingScreen;
    [SerializeField] private bool isLoadingQueuedScenes = false;
    [SerializeField] public float transitionSpeedMultiplier = 1f;

    public static Manager_OverlayLoadingScreen instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Overlay Loading Screen Manager in the scene.");
        }
        instance = this;

        if (isLoadingQueuedScenes)
        {
            StartCoroutine(LoadQueuedScene());
        }
    }

    [ContextMenu("Close Loading Screen")]
    public void OpenLoadingScreen()
    {
        PrepareOpenLoadingScreen();
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500f, 1.2f * transitionSpeedMultiplier).setEaseInQuart().setDelay(1.5f * transitionSpeedMultiplier);
    }

    [ContextMenu("Prepare Close Loading Screen")]
    public void PrepareOpenLoadingScreen()
    {
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0, 0);
    }

    [ContextMenu("Open Loading Screen")]
    public void CloseLoadingScreen()
    {
        PrepareCloseLoadingScreen();
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), 0f, 1.2f * transitionSpeedMultiplier).setEaseInQuart().setDelay(1f * transitionSpeedMultiplier);
    }

    [ContextMenu("Prepare Open Loading Screen")]
    public void PrepareCloseLoadingScreen()
    {
        LeanTween.moveX(blackScreen.GetComponent<RectTransform>(), -2500, 0);
    }

    public void LoadTrueLoadingScreen(string sceneName)
    {
        _sceneQueue.UnqueueAllScenes();
        _sceneQueue.QueueScene(sceneName);
        _sceneQueue.LoadScene(scene_trueLoadingScreen);
    }

    public IEnumerator LoadQueuedScene()
    {
        yield return new WaitForSeconds(2);
        _sceneQueue.LoadQueuedScenes();
    }
}
