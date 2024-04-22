using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager_PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enabledGroup;
    [SerializeField] private GameObject mainPanelGroup;
    [SerializeField] private bool isTransitioning;
    [SerializeField] private GameObject chunkfishDisk;
    [SerializeField] private GameObject chunkfishDiskNeedle;

    public bool isUnpausable;

    [Header("Screen Follow Mouse References")]
    [SerializeField] private Vector2 parallaxOriginPoint;
    [SerializeField] private float parallaxScale;
    [SerializeField] private float lerpScale;
    [SerializeField] private Vector2 offset;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    public static Manager_PauseMenu instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Pause Menu Manager in the scene.");
        }
        instance = this;

        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Interact.Pause.performed += OnInteractPerformed;
        playerInput.Enable();
    }

    private void OnDestroy()
    {
        //// Unubscribes to Unity's input system
        playerInput.Interact.Pause.performed -= OnInteractPerformed;
        playerInput.Disable();

        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        OnPause();
    }

    public void OnPause() 
    {
        if (!isUnpausable)
        {
            TogglePauseButtonPress();
        }
    }

    private void TogglePauseButtonPress()
    {
        if (enabledGroup.activeSelf == true)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            EndPauseMenu();
        }
        else
        {
            enabledGroup.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;
            InitiatePauseMenu();
        }
    }

    private void InitiatePauseMenu()
    {
        ExpandPauseMenu();
        ChunkfishDiskInitiate();
    }

    private void EndPauseMenu()
    {
        StartCoroutine(ShrinkPauseMenu());
    }

    private IEnumerator PauseMenuMove()
    {
        ScreenFollowMouseUpdate();

        yield return null;
        if (enabledGroup.activeSelf == true)
        {
            StartCoroutine(PauseMenuMove());
        }
    }

    private void ScreenFollowMouseUpdate()
    {
        parallaxOriginPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        GetParallax();

        float xParallax = offset.x * parallaxScale;
        float yParallax = offset.y * parallaxScale;

        float desiredX = Mathf.Lerp(mainPanelGroup.transform.position.x, parallaxOriginPoint.x + xParallax, lerpScale);
        float desiredY = Mathf.Lerp(mainPanelGroup.transform.position.y, parallaxOriginPoint.y + yParallax, lerpScale);
        Vector2 desiredPos = new Vector2(desiredX, desiredY);

        mainPanelGroup.transform.position = desiredPos;
    }

    private void GetParallax()
    {
        Vector3 mousePosition = Input.mousePosition;
        offset = GetOffsetFromCenterScreen(parallaxOriginPoint, mousePosition);
    }

    private Vector2 GetOffsetFromCenterScreen(Vector2 pos1, Vector2 pos2)
    {
        float distanceX = pos2.x - pos1.x;
        float distanceY = pos2.y - pos1.y;
        Vector2 distance = new Vector2(distanceX, distanceY);

        return distance;
    }

    private void ChunkfishDiskInitiate()
    {
        LeanTween.rotateZ(chunkfishDisk, 0, 0).setIgnoreTimeScale(true);
        LeanTween.rotateZ(chunkfishDiskNeedle, 0, 0).setIgnoreTimeScale(true);
        LeanTween.rotateAroundLocal(chunkfishDisk, Vector3.forward, -360, 4f).setLoopClamp().setIgnoreTimeScale(true).setDelay(0.1f);
        LeanTween.rotateAroundLocal(chunkfishDiskNeedle, Vector3.forward, 4, 1f).setLoopClamp().setIgnoreTimeScale(true).setEaseInBounce().setDelay(0.1f);

    }

    private void ChunkfishDiskEnd()
    {
        LeanTween.cancel(chunkfishDisk);
        LeanTween.cancel(chunkfishDiskNeedle);
    }

    private void ExpandPauseMenu()
    {
        if (isTransitioning == false)
        {
            isTransitioning = true;
            mainPanelGroup.transform.localScale = Vector3.zero;
            LeanTween.scale(mainPanelGroup, Vector3.one, 0.1f).setIgnoreTimeScale(true);

            StopAllCoroutines();
            StartCoroutine(PauseMenuMove());
        }
    }

    private IEnumerator ShrinkPauseMenu()
    {
        if (isTransitioning)
        {
            ChunkfishDiskEnd();

            mainPanelGroup.transform.localScale = Vector3.one;
            LeanTween.scale(mainPanelGroup, Vector3.zero, 0.1f).setIgnoreTimeScale(true);
            yield return new WaitForSeconds(0.1f);
            isTransitioning = false;
            enabledGroup.SetActive(false);
        }
    }
}
