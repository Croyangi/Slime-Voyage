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

    [SerializeField] private GameObject pulley;
    [SerializeField] private GameObject gear1;
    [SerializeField] private GameObject gear2;

    [SerializeField] private GameObject slime;

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
    [SerializeField] private AudioClip music_elevatorCutscene;

    [SerializeField] private Image fadeOutTransition;
    [SerializeField] private float elapsedTime;

    [SerializeField] private GameObject fallingParticles;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private ScriptObj_SceneName scene_loadingScreen;
    [SerializeField] private ScriptObj_SceneName scene_activeScene;

    private void Awake()
    {
        StartCoroutine(LoadLoadingScreen());
        fadeOutTransition.gameObject.SetActive(true);
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene.name));
        Manager_PlayerState.instance.isResetDeathOn = false;
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();

            // Initiate cutscene
            StartCoroutine(InitiateFadeOutTransition());
            StartCoroutine(InitiateElevatorCutscene());
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
            StartCoroutine(InitiateFadeOutTransition());
            StartCoroutine(InitiateElevatorCutscene());
        }
    }

    private IEnumerator InitiateFadeOutTransition()
    {
        yield return new WaitForSeconds(5f);
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

    private IEnumerator InitiateElevatorCutscene()
    {
        // Music
        Manager_SFXPlayer.instance.PlaySFXClip(music_elevatorCutscene, transform, 1, false, Manager_AudioMixer.instance.mixer_music);

        // Start
        LeanTween.rotateAround(pulley, Vector3.forward, 360, 2.5f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, 360, 2f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, 360, 2f).setLoopClamp();

        LeanTween.moveY(elevator, elevatorRiseYPos, elevatorRiseTime);
        yield return new WaitForSeconds(elevatorRiseTime);

        // Sudden Stop
        StartCoroutine(FlickerLight());

        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 0.5f, 1f).setEaseInBounce();
        LeanTween.cancel(pulley);
        LeanTween.cancel(gear1);
        LeanTween.cancel(gear2);
        yield return new WaitForSeconds(2f);

        // Slow fall down
        LeanTween.moveY(elevator, elevator.transform.position.y - elevatorSlowFallYPos, elevatorSlowFallTime).setEaseInQuad();
        LeanTween.rotateAround(pulley, Vector3.forward, -360, 2.5f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, -360, 2f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, -360, 2f).setLoopClamp();
        yield return new WaitForSeconds(elevatorSlowFallTime);

        // Fall
        fallingParticles.SetActive(true);

        InitiateRedLights();

        slime.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        slime.GetComponent<Rigidbody2D>().gravityScale = 0;
        StartCoroutine(ApplyFallingVelocity());

        LeanTween.moveY(elevator, elevator.transform.position.y - elevatorFallYPos, elevatorFallTime);

        LeanTween.cancel(pulley);
        LeanTween.cancel(gear1);
        LeanTween.cancel(gear2);
        LeanTween.rotateAround(pulley, Vector3.forward, -360, 0.1f).setLoopClamp();
        LeanTween.rotateAround(gear1, Vector3.forward, -360, 0.1f).setLoopClamp();
        LeanTween.rotateAround(gear2, Vector3.forward, -360, 0.1f).setLoopClamp();
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
        slime.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
        yield return new WaitForFixedUpdate();
        StartCoroutine(ApplyFallingVelocity());
    }
}
