using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_WarehousePrologue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image closingTransition;
    [SerializeField] private bool isClosing = false;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_theWarehouse;
    [SerializeField] private string scene_overlayLoadingScreen;

    private void Awake()
    {
        _sceneQueue.LoadScene(scene_overlayLoadingScreen, true);
        StartCoroutine(DelayedAwake());

        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 0f), 3f).setEaseInCubic();
        StartCoroutine(OnWarehouseProloguePlay());
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForFixedUpdate();
        Manager_LoadingScreen.instance.OpenLoadingScreen();
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
        StartCoroutine(LoadWarehouse());
    }

    private IEnumerator SkipWarehousePrologue()
    {
        isClosing = true;
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 1f), 0.5f).setEaseInCubic();

        yield return new WaitForSeconds(1);
        StartCoroutine(LoadWarehouse());
    }

    private IEnumerator LoadWarehouse()
    {
        Manager_LoadingScreen.instance.CloseLoadingScreen();
        yield return new WaitForSeconds(4);
        _sceneQueue.LoadScene(scene_theWarehouse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isClosing == false)
        {
            StartCoroutine(SkipWarehousePrologue());
        }
    }


}

