using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_WarehousePrologue : MonoBehaviour
{
    [SerializeField] private Image closingTransition;
    [SerializeField] private RoomQueue _roomQueue;
    [SerializeField] private bool isClosing = false;

    private void Awake()
    {
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 0f), 2f).setEaseInCubic();
        StartCoroutine(OnWarehouseProloguePlay());
    }

    private IEnumerator OnWarehouseProloguePlay()
    {

        yield return new WaitForSeconds(22);
        if (isClosing == false)
        {
            StartCoroutine(EndWarehousePrologue());
        }
    }

    private IEnumerator EndWarehousePrologue()
    {
        isClosing = true;
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 1f), 2f).setEaseInCubic();

        yield return new WaitForSeconds(3);
        LoadRoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isClosing == false)
        {
            StartCoroutine(EndWarehousePrologue());
        }
    }

    private void LoadRoom()
    {
        _roomQueue.LoadRoom("BootLoader_Warehouse");
    }


}

