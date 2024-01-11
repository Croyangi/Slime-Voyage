using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChunkedLogo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float timer;
    [SerializeField] private float whiteScreenTransitionTimer;
    [SerializeField] private float mainMenuTimer;

    [SerializeField] private GameObject whiteSquareTransition;
    [SerializeField] private RoomQueue _roomQueue;

    private void Start()
    {
        SceneManager.LoadScene("Bootloader_Global", LoadSceneMode.Additive);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (whiteScreenTransitionTimer < timer)
        {
            WhiteScreenTransition();
        }

        if (mainMenuTimer < timer)
        {
            LoadLogoMenu();
        }

        if (Input.anyKey)
        {
            LoadLogoMenu();
        }
    }

    private void WhiteScreenTransition()
    {
        whiteSquareTransition.SetActive(true);
    }

    private void LoadLogoMenu()
    {
        _roomQueue.LoadRoom("LogoMenu");
    }

}
