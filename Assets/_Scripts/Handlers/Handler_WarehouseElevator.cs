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
    [SerializeField] private GameObject prompt;
    [SerializeField] private Image closingTransition;

    [SerializeField] private BootLoader_Warehouse _warehouse;

    [Header("Elevator Pieces")]
    [SerializeField] private GameObject chainedGear;
    [SerializeField] private GameObject elevator;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float amplitude = 0;
    [SerializeField] private float frequency = 1;
    [SerializeField] private float amplitudeRotate = 0;
    [SerializeField] private float frequencyRotate = 1;
    [SerializeField] private float time;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_demoEnd;
    [SerializeField] private string scene_deloadedScene;

    public IEnumerator ElevatorPlateMove()
    {
        MovementMath();

        yield return new WaitForFixedUpdate();
        if (elevatorPanel.activeSelf == true)
        {
            StartCoroutine(ElevatorPlateMove());
        }
    }

    private void MovementMath()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time * frequency) * amplitude;
        float rotateZ = Mathf.Sin(time * frequencyRotate) * amplitudeRotate;
        elevatorPanel.transform.position = new Vector2(elevatorPanel.transform.position.x, elevatorPanel.transform.position.y + y);
        elevatorPanel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
    }

    public void OnElevatorUpButton()
    {
        Manager_SpeedrunTimer.instance.EndSpeedrunTimer();
        StartCoroutine(OnElevatorButtonUpInitiate());
    }

    public void OnElevatorDownButton()
    {
        Manager_SpeedrunTimer.instance.EndSpeedrunTimer();
        StartCoroutine(OnElevatorButtonDownInitiate());
    }

    private IEnumerator OnElevatorButtonUpInitiate()
    {
        prompt.SetActive(false);
        canvas_elevator.SetActive(false);
        Manager_PlayerState.instance.SetInputStall(false);

        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 0.5f, 1f).setEaseInBounce();
        yield return new WaitForSeconds(2f);
        //StartCoroutine(ElevatorUpButtonAnimation());
        LeanTween.rotateAround(chainedGear, Vector3.forward, 360, 2.5f).setLoopClamp();
        LeanTween.moveLocalY(elevator, elevator.transform.position.y + 100, 10).setEaseInCubic();

        yield return new WaitForSeconds(6f);

        // Transition screen
        _warehouse.OnWarehouseComplete();
    }

    private IEnumerator OnElevatorButtonDownInitiate()
    {
        prompt.SetActive(false);
        canvas_elevator.SetActive(false);
        Manager_PlayerState.instance.SetInputStall(false);

        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 0.5f, 0.3f).setEaseInBounce();
        yield return new WaitForSeconds(2f);
        //StartCoroutine(ElevatorDownButtonAnimation());
        LeanTween.rotateAround(chainedGear, Vector3.forward, -360, 2.5f).setLoopClamp();
        LeanTween.moveLocalY(elevator, elevator.transform.position.y - 150, 4);

        yield return new WaitForSeconds(3f);

        // Transition screen
        _warehouse.OnWarehouseComplete();
    }

    public void InitiateElevatorPanel()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;
            canvas_elevator.SetActive(true);
            elevatorPanel.transform.localScale = Vector3.zero;
            LeanTween.scale(elevatorPanel, Vector3.one, 0.1f);

            StopAllCoroutines();
            StartCoroutine(ElevatorPlateMove());
        }
    }

    public IEnumerator EndElevatorPanel()
    {
        if (isTransitioning == true)
        {
            elevatorPanel.transform.localScale = Vector3.one;
            LeanTween.scale(elevatorPanel, Vector3.zero, 0.1f);
            yield return new WaitForSeconds(0.1f);
            canvas_elevator.SetActive(false);
            isTransitioning = false;
        }
    }
}
