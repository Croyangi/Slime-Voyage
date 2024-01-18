using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_PauseButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuCanvas;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    private void Awake()
    {
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
    }

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        TogglePauseButtonPress();
    }

    public void TogglePauseButtonPress()
    {
        if (pauseMenuCanvas.activeSelf == true) 
        { 
            DisablePauseScreen();
            ResumeGame();
        } else
        {
            Manager_DialogueHandler.instance.ForceQuitDialogue();
            EnablePauseScreen();
            PauseGame();
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void EnablePauseScreen()
    {
        pauseMenuCanvas.SetActive(true);
    }

    private void DisablePauseScreen()
    {
        pauseMenuCanvas.SetActive(false);
    }
}
