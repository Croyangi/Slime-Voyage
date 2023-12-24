using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_PauseButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuCanvas;

    public void TogglePauseButtonPress()
    {
        if (pauseMenuCanvas.activeSelf == true) 
        { 
            DisablePauseScreen();
            ResumeGame();
        } else
        {
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

    private void OnDestroy()
    {
        Time.timeScale = 1;   
    }
}
