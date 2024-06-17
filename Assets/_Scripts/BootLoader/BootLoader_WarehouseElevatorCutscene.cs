using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BootLoader_WarehouseElevatorCutscene : MonoBehaviour
{
    [Header("Elevator Parts")]
    [SerializeField] private GameObject elevator;
    [SerializeField] private float elevatorRiseYPos;
    [SerializeField] private float elevatorRiseTime;

    [SerializeField] private float elevatorSlowFallYPos;
    [SerializeField] private float elevatorSlowFallTime;

    [SerializeField] private float elevatorFallYPos;
    [SerializeField] private float elevatorFallTime;

    [SerializeField] private float transitionYpos;

    [SerializeField] private GameObject pulley;
    [SerializeField] private GameObject gear1;
    [SerializeField] private GameObject gear2;

    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject newspaperStepspike;

    [Header("Elevator Light")]
    [SerializeField] private GameObject elevatorLight;
    [SerializeField] private SpriteRenderer elevatorLightSr;
    [SerializeField] private Light2D elevatorLight2D;
    [SerializeField] private Sprite elevatorLightOn;
    [SerializeField] private Sprite elevatorLightOff;

    [SerializeField] private GameObject elevatorRedLight;
    [SerializeField] private GameObject elevatorRotatingRedLights;
    [SerializeField] private Sprite elevatorRedLightOn;

    [Header("References")]
    [SerializeField] private AudioClip music_elevatorCutsceneBeginning;
    [SerializeField] private AudioClip music_elevatorCutsceneFall;

    [SerializeField] private Image fadeOutTransition;
    [SerializeField] private float elapsedTime;

    [SerializeField] private GameObject fallingParticles;

    [SerializeField] private GameObject warehouseBackround;
    [SerializeField] private GameObject basementBackround;

    [SerializeField] private NPC_NewspaperStepspike _newspaperStepspike;
    [SerializeField] private GameObject dialoguePrompt;

    [SerializeField] private bool isCutsceneReady;

    [SerializeField] private bool isCutoff;
    [SerializeField] private bool isSkippable;
    [SerializeField] private GameObject skipCutscene;

    [SerializeField] private GameObject risingCinemachine;
    [SerializeField] private GameObject fallingCinemachine;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;
    [SerializeField] private ScriptObj_SceneName scene_activeScene;
    [SerializeField] private ScriptObj_SceneName scene_loadedScene;
    [SerializeField] private ScriptObj_SceneName scene_deloadedScene;

    private void Awake()
    {
        StartCoroutine(LoadLoadingScreen());
        PrepareOpenSkipCutscene();
        fadeOutTransition.gameObject.SetActive(true);

        risingCinemachine.SetActive(true);
        fallingCinemachine.SetActive(false);
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene.name));
        Manager_PlayerState.instance.isResetDeathOn = false;
        StartCoroutine(OnCutsceneStandby());
    }

    private void Update()
    {
        // Skip cutscene
        if (Input.GetKeyUp(KeyCode.P) && isCutoff == false && isSkippable == true)
        {
            isCutoff = true;
            SkipCutscene();
        }
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
        OnElevatorCutsceneComplete();
    }

    private IEnumerator OnCutsceneStandby()
    {
        yield return null;
        if (isCutsceneReady)
        {
            StartCoroutine(InitiateFadeOutTransition());
            StartCoroutine(InitiateElevatorCutscene());

            yield return new WaitForSeconds(5f);
            isSkippable = true;

            OpenSkipCutscene();
        } else
        {
            StartCoroutine(OnCutsceneStandby());
        }
    }

    private void OnElevatorCutsceneComplete()
    {
        Manager_LoadingScreen.instance.InitiateLoadSceneTransfer(scene_loadedScene.name, scene_deloadedScene.name);
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();

            // Initiate cutscene
            isCutsceneReady = true;
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen.name, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Finished");

            Manager_LoadingScreen.instance.OpenLoadingScreen();

            // Initiate cutscene
            isCutsceneReady = true;
        }
    }

    private IEnumerator InitiateFadeOutTransition()
    {
        yield return new WaitForSeconds(6f);
        StartCoroutine(FadeOutTransition());
    }

    private IEnumerator FadeOutTransition()
    {
        elapsedTime += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 6f);

        fadeOutTransition.color = new Color(fadeOutTransition.color.r, fadeOutTransition.color.g, fadeOutTransition.color.b, alpha);

        yield return new WaitForFixedUpdate();
        if (fadeOutTransition.color.a > 0f)
        {
            StartCoroutine(FadeOutTransition());
        }
    }

    private void InitiateFadeInTransition()
    {
        elapsedTime = 0f;
        fadeOutTransition.color = new Color(fadeOutTransition.color.r, fadeOutTransition.color.g, fadeOutTransition.color.b, 0f);
        StartCoroutine(FadeInTransition());
    }

    private IEnumerator FadeInTransition()
    {
        elapsedTime += Time.deltaTime;

        float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 2f);

        fadeOutTransition.color = new Color(fadeOutTransition.color.r, fadeOutTransition.color.g, fadeOutTransition.color.b, alpha);

        yield return new WaitForFixedUpdate();
        if (fadeOutTransition.color.a < 1f)
        {
            StartCoroutine(FadeInTransition());
        }
    }

    private IEnumerator InitiateElevatorCutscene()
    {
        // Music
        Manager_SFXPlayer.instance.PlaySFXClip(music_elevatorCutsceneBeginning, transform, 1, false, Manager_AudioMixer.instance.mixer_music);

        // Start
        LeanTween.rotateAround(pulley, Vector3.forward, 360, 2.5f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, 360, 2f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, 360, 2f).setLoopClamp();

        LeanTween.moveY(elevator, elevatorRiseYPos, music_elevatorCutsceneBeginning.length - 1.7f);
        yield return new WaitForSeconds(music_elevatorCutsceneBeginning.length - 1.7f);

        // Sudden Stop
        StartCoroutine(FlickerLight());
        _newspaperStepspike.OnElevatorStallDialogueInteraction();

        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 0.5f, 1f).setEaseInBounce();
        LeanTween.cancel(pulley);
        LeanTween.cancel(gear1);
        LeanTween.cancel(gear2);
        yield return new WaitForSeconds(7f);

        // Slow fall down
        Manager_SFXPlayer.instance.PlaySFXClip(music_elevatorCutsceneFall, transform, 1, false, Manager_AudioMixer.instance.mixer_music);
        yield return new WaitForSeconds(0.2f);

        LeanTween.moveY(elevator, elevator.transform.position.y - elevatorSlowFallYPos, elevatorSlowFallTime).setEaseInQuad();
        LeanTween.rotateAround(pulley, Vector3.forward, -360, 2.5f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, -360, 2f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, -360, 2f).setLoopClamp();
        yield return new WaitForSeconds(elevatorSlowFallTime);

        // Fall
        risingCinemachine.SetActive(false);
        fallingCinemachine.SetActive(true);

        fallingParticles.SetActive(true);
        _newspaperStepspike.OnElevatorFallDialogueInteraction();

        InitiateRedLights();
        StartCoroutine(TransitionBasementBackround());

        StartCoroutine(ApplyFallingVelocity());

        LeanTween.moveY(elevator, elevator.transform.position.y - elevatorFallYPos, elevatorFallTime);

        LeanTween.cancel(pulley);
        LeanTween.cancel(gear1);
        LeanTween.cancel(gear2);
        LeanTween.rotateAround(pulley, Vector3.forward, -360, 0.1f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, -360, 0.1f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, -360, 0.1f).setLoopClamp();

        yield return new WaitForSeconds(elevatorFallTime);
        yield return new WaitForSeconds(5f);
        OnElevatorCutsceneComplete();

    }

    private void InitiateRedLights()
    {
        elevatorLight.SetActive(false);
        elevatorRedLight.SetActive(true);
        LeanTween.rotateAround(elevatorRotatingRedLights, Vector3.up, 360, 2f).setLoopClamp();
    }

    private IEnumerator FlickerLight()
    {
        ToggleLight(false);
        yield return new WaitForSeconds(0.3f);
        ToggleLight(true);
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 3; i++)
        {
            ToggleLight(false);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            ToggleLight(true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        yield return new WaitForSeconds(0.1f);
        ToggleLight(false);
    }

    private void ToggleLight(bool state)
    {
        elevatorLight2D.gameObject.SetActive(state);

        if (state)
        {
            elevatorLightSr.sprite = elevatorLightOn;
        } else
        {
            elevatorLightSr.sprite = elevatorLightOff;
        }
    }

    private IEnumerator ApplyFallingVelocity()
    {
        slime.GetComponent<Rigidbody2D>().velocity = Vector2.up * 30f;
        newspaperStepspike.GetComponent<Rigidbody2D>().velocity = Vector2.up * 3f;
        newspaperStepspike.GetComponent<Rigidbody2D>().angularVelocity = 10f;
        yield return new WaitForFixedUpdate();
        StartCoroutine(ApplyFallingVelocity());
    }

    private IEnumerator TransitionBasementBackround()
    {
        yield return new WaitForFixedUpdate();
        if (elevator.transform.position.y > transitionYpos)
        {
            StartCoroutine(TransitionBasementBackround());
        } else
        {
            warehouseBackround.SetActive(false);
            basementBackround.SetActive(true);
            InitiateFadeInTransition();
            dialoguePrompt.SetActive(false);
        }
    }
}
