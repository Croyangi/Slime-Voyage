using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_PauseButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enabledGroup;
    [SerializeField] private Image image_pauseButton;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
        //image_pauseButton.color = new Color(image_pauseButton.color.r, image_pauseButton.color.g, image_pauseButton.color.b, 0.5f);

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

    private void OnMouseEnter()
    {
        //image_pauseButton.color = new Color(image_pauseButton.color.r, image_pauseButton.color.g, image_pauseButton.color.b, 1f);
    }

    private void OnMouseExit()
    {
        //image_pauseButton.color = new Color(image_pauseButton.color.r, image_pauseButton.color.g, image_pauseButton.color.b, 0.5f);
    }

    public void TogglePauseButtonPress()
    {
        if (enabledGroup.activeSelf == true) 
        { 
            DisablePauseScreen();
            ResumeGame();
            //Manager_PauseMenu.instance.EndPauseMenu();
        } else
        {
            //image_pauseButton.color = new Color(image_pauseButton.color.r, image_pauseButton.color.g, image_pauseButton.color.b, 1f);
            EnablePauseScreen();
            PauseGame();
            //Manager_PauseMenu.instance.InitiatePauseMenu();
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
        enabledGroup.SetActive(true);
    }

    private void DisablePauseScreen()
    {
        enabledGroup.SetActive(false);
    }
}
