using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Handler_WarehouseElevator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject elevatorPanel;
    [SerializeField] private bool isTransitioning = false;
    [SerializeField] private GameObject canvas_elevator;
    [SerializeField] private Handler_WarehouseElevatorCutscene _elevatorCutscene;
    [SerializeField] private GameObject dialoguePrompt;

    [Header("Broken Button")]
    [SerializeField] private AudioClip sfx_brokenButton;
    [SerializeField] private Image brokenButtonOutline;
    [SerializeField] private Color brokenButtonOutlineOn = new Color(1f, 0.78f, 0f, 1f);
    [SerializeField] private Color brokenButtonOutlineOff = new Color(0.36f, 0.36f, 0.36f, 1f);

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float amplitude = 0;
    [SerializeField] private float frequency = 1;
    [SerializeField] private float amplitudeRotate = 0;
    [SerializeField] private float frequencyRotate = 1;
    [SerializeField] private float time;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;

    // Called by dialogue prompt
    public void InitiateElevatorPanel()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;

            canvas_elevator.SetActive(true);
            elevatorPanel.transform.localScale = Vector3.zero;
            LeanTween.scale(elevatorPanel, Vector3.one, 0.1f);

            StopAllCoroutines();

            // Looping VFX
            StartCoroutine(ElevatorPlateMove());
            StartCoroutine(BrokenButtonOutlineFlicker());
        }
    }


    // When exited
    public IEnumerator EndElevatorPanel()
    {
        if (isTransitioning == true)
        {
            // Shrinking VFX takes 0.1 seconds, only then allowed to re-open, cause it messes up the scales
            elevatorPanel.transform.localScale = Vector3.one;
            LeanTween.scale(elevatorPanel, Vector3.zero, 0.1f);

            yield return new WaitForSeconds(0.1f);

            canvas_elevator.SetActive(false);
            isTransitioning = false;
        }
    }

    // Looping hovering VFX
    public IEnumerator ElevatorPlateMove()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time * frequency) * amplitude;
        float rotateZ = Mathf.Sin(time * frequencyRotate) * amplitudeRotate;
        elevatorPanel.transform.position = new Vector2(elevatorPanel.transform.position.x, elevatorPanel.transform.position.y + y);
        elevatorPanel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));

        yield return new WaitForFixedUpdate();
        if (isTransitioning)
        {
            StartCoroutine(ElevatorPlateMove());
        }
    }

    public IEnumerator BrokenButtonOutlineFlicker()
    {
        brokenButtonOutline.color = brokenButtonOutlineOn;
        yield return new WaitForSeconds(Random.Range(0f, 0.7f));

        brokenButtonOutline.color = brokenButtonOutlineOff;
        yield return new WaitForSeconds(Random.Range(0f, 0.7f));

        yield return new WaitForFixedUpdate();
        if (isTransitioning)
        {
            StartCoroutine(BrokenButtonOutlineFlicker());
        }
    }

    public void OnElevatorUpButton()
    {
        Manager_PlayerState.instance.SetResetDeath(false);
        Manager_SpeedrunTimer.instance.EndSpeedrunTimer();

        // Cleanup
        canvas_elevator.SetActive(false);
        dialoguePrompt.SetActive(false);

        _elevatorCutscene.InitiateElevatorCutscene();
    }

    public void OnElevatorDownButton()
    {
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_brokenButton, transform, 1, false, Manager_AudioMixer.instance.mixer_sfx, isPitchShifted: true, 0.1f);
    }
}
