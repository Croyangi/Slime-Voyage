using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BootLoader_WarehousePrologue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource music_cutscene;
    [SerializeField] private VideoPlayer video_cutscene;
    [SerializeField] private float cutsceneCutoffTime;
    [SerializeField] private bool isCutoff;
    [SerializeField] private bool isSkippable;
    [SerializeField] private GameObject skipCutscene;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;
    [SerializeField] private string scene_theWarehouse;
    [SerializeField] private string scene_deloadedScene;

    private void Awake()
    {
        PrepareOpenSkipCutscene();
        music_cutscene.ignoreListenerPause = true;

        StartCoroutine(LoadLoadingScreen());

        StartCoroutine(LoadCutscene());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    private IEnumerator LoadLoadingScreen()
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

    private void Update()
    {
        // Transfers scene based on cutoff time
        if (video_cutscene.time > video_cutscene.length - cutsceneCutoffTime && isCutoff == false)
        {
            isCutoff = true;
            LoadWarehouse();
        }

        // Skip cutscene
        if (Input.GetKeyUp(KeyCode.X) && isCutoff == false && isSkippable == true) 
        {
            isCutoff = true;
            SkipCutscene();
        }
    }

    // Just delays it, so the loading screen doesn't overshadow it
    private IEnumerator LoadCutscene()
    {
        yield return new WaitForSeconds(1f);
        music_cutscene.Play();
        video_cutscene.Play();
        yield return new WaitForSeconds(2f);
        isSkippable = true;
        OpenSkipCutscene();
    }

    // UI vfx for opening the skip cutscene object
    private void OpenSkipCutscene()
    {
        LeanTween.moveX(skipCutscene.GetComponent<RectTransform>(), -960f, 2f).setEaseInOutBack().setIgnoreTimeScale(true);
    }

    private void CloseSkipCutscene()
    {
        LeanTween.moveX(skipCutscene.GetComponent<RectTransform>(), -1300f, 2f).setEaseInOutBack().setIgnoreTimeScale(true);
    }

    // UI vfx for opening the skip cutscene object
    private void PrepareOpenSkipCutscene()
    {
        LeanTween.moveX(skipCutscene.GetComponent<RectTransform>(), -1300f, 0f).setIgnoreTimeScale(true);
    }

    private void SkipCutscene()
    {
        CloseSkipCutscene();
        LoadWarehouse();
    }

    private void LoadWarehouse()
    {
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_theWarehouse, scene_deloadedScene);
    }


}

