using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Handler_LogoStartup : MonoBehaviour
{
    [Header("Logo Startup References")]
    [SerializeField] private Image whiteScreenTransition;

    [SerializeField] private float whiteScreen_target;
    [SerializeField] private float whiteScreen_transitionTime;

    [SerializeField] private AudioClip audioClip_slimeVoyageMainTheme;
    [SerializeField] private AudioSource audioSource_slimeVoyageMainTheme;
    [SerializeField] private float mainTheme_target;
    [SerializeField] private float mainTheme_transitionTime;

    [SerializeField] private bool canSkip = true;
    [SerializeField] private int skipCounter = 0;

    [Header("References")]
    [SerializeField] private Handler_MainMenu _mainMenu;
    [SerializeField] private GameObject setup;

    private void Awake()
    {
        StartCoroutine(StartTransitionMainMenu());

        setup.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown && canSkip == true)
        {
            skipCounter++;
            if (skipCounter >= 3)
            {
                canSkip = false;
                OnMainMenu();
            }
        }
    }

    private IEnumerator StartTransitionMainMenu()
    {
        audioSource_slimeVoyageMainTheme.time = 0f;
        audioSource_slimeVoyageMainTheme.Play();
        StartCoroutine(FadeInMainTheme());

        yield return new WaitForSeconds(5f);
        whiteScreenTransition.enabled = true;
        StartCoroutine(FadeInWhiteScreen());

        yield return new WaitForSeconds(3f);
        OnMainMenu();
    }

    private IEnumerator FadeInMainTheme()
    {
        float volume = audioSource_slimeVoyageMainTheme.volume;
        float newVolume = Mathf.MoveTowards(volume, mainTheme_target, Time.deltaTime / mainTheme_transitionTime);
        audioSource_slimeVoyageMainTheme.volume = newVolume;

        if (newVolume != mainTheme_target)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(FadeInMainTheme());
        }
    }

    private IEnumerator FadeInWhiteScreen()
    {
        Color whiteScreenColor = whiteScreenTransition.color;
        float newAlpha = Mathf.MoveTowards(whiteScreenColor.a, whiteScreen_target, Time.deltaTime / whiteScreen_transitionTime);
        whiteScreenTransition.color = new Color(whiteScreenColor.r, whiteScreenColor.g, whiteScreenColor.b, newAlpha);

        if (newAlpha != whiteScreen_target)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(FadeInWhiteScreen());
        }
    }

    private void OnMainMenu()
    {
        StopAllCoroutines();

        setup.SetActive(false);
        _mainMenu.OnMainMenuSetup();
        canSkip = false;

        whiteScreenTransition.enabled = false;
    }
}
