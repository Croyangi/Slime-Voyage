using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ResumeButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuCanvas;

    public void OnResumeButtonPress()
    {
        ResumeGame();
        DisablePauseScreen();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
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
