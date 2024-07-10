using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Handler_MainMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_mainMenu;
    [SerializeField] private ScriptObj_SceneName scene_warehouseDioramaMenu;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;

    [Header("Tabs")]
    [SerializeField] private GameObject[] tabs;

    [Header("References")]
    [SerializeField] private Canvas setup_canvas;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject supportUs;
    [SerializeField] private AudioClip sfx_onPlayClicked;
    [SerializeField] private bool isTransitioning;


    private void Awake()
    {
        setup_canvas.enabled = false;

        credits.SetActive(false);
        controls.SetActive(false);
        supportUs.SetActive(false);

        StartCoroutine(LoadLoadingScreen());
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.PrepareCloseLoadingScreen();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen.name, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            Manager_LoadingScreen.instance.PrepareCloseLoadingScreen();
        }
    }

    // Close and then transition
    private void LoadWarehouseDioramaMenu()
    {
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_warehouseDioramaMenu.name);
    }

    // Called by another script to enable canvas
    public void OnMainMenuSetup()
    {
        setup_canvas.enabled = true;
    }

    // Set tab active
    public void SetActiveTabVFX(int index)
    {
        GameObject tab = tabs[index];

        if (index == 4)
        {
            LeanTween.moveX(tab.GetComponent<RectTransform>(), 820f, 0.5f).setEaseInBack().setEaseOutBounce();
        } else
        {
            LeanTween.moveX(tab.GetComponent<RectTransform>(), 330f, 0.5f).setEaseInBack().setEaseOutBounce();
        }
    }

    // Set tab deactive
    public void SetDeactiveTabVFX(int index)
    {
        GameObject tab = tabs[index];

        if (index == 4)
        {
            LeanTween.moveX(tab.GetComponent<RectTransform>(), 900f, 0.5f).setEaseInBack().setEaseOutBounce();
        }
        else
        {
            LeanTween.moveX(tab.GetComponent<RectTransform>(), 250f, 0.5f).setEaseInBack().setEaseOutBounce();
        }
    }

    //// Buttons
    public void OnPlayButtonPressed()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;

            LoadWarehouseDioramaMenu();
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_onPlayClicked, transform, 1f, false, Manager_AudioMixer.instance.mixer_sfx);
        }
    }

    public void OnControlsButtonPressed()
    {
        controls.SetActive(!controls.activeSelf);
        credits.SetActive(false);
        supportUs.SetActive(false);
    }


    public void OnCreditsButtonPressed()
    {
        credits.SetActive(!credits.activeSelf);
        controls.SetActive(false);
        supportUs.SetActive(false);
    }

    public void OnSupportUsButtonPressed()
    {
        supportUs.SetActive(!supportUs.activeSelf);
        credits.SetActive(false);
        controls.SetActive(false);
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
