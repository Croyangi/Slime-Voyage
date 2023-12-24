using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialPrologueCinematic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float timer;
    [SerializeField] private float[] timeTriggers;
    [SerializeField] private int timeTriggerCurrent;
    [SerializeField] private RoomQueue _roomQueue;
    [SerializeField] private GameObject whiteSquareTransition;
    [SerializeField] private float cameraSizeTarget;
    [SerializeField] private float cameraSizeTransitionSpeed;

    private void Awake()
    {
        cameraSizeTarget = gameObject.GetComponent<Camera>().orthographicSize;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timeTriggerCurrent < timeTriggers.Length)
        {
            if (timer > timeTriggers[timeTriggerCurrent])
            {
                AnimationSetter(timeTriggerCurrent);
                timeTriggerCurrent++;
            }
        }

        gameObject.GetComponent<Camera>().orthographicSize = Mathf.MoveTowards(gameObject.GetComponent<Camera>().orthographicSize, cameraSizeTarget, cameraSizeTransitionSpeed * Time.deltaTime);
    }

    private void AnimationSetter(int id)
    {
        switch (id)
        {
            case 0:
                MoveTo2nd();
                break;
            case 1:
                MoveTo3rd();
                break;
            case 2:
                MoveTo4th();
                break;
            case 3:
                MoveTo5th();
                break;
            case 4:
                MoveTo6th();
                break;
            case 5:
                MoveTo7th();
                break;
            case 6:
                MoveTo8th();
                break;
            case 7:
                MoveTo9th();
                break;
            case 8:
                WhiteScreenTransition();
                break;
            case 9:
                LoadMainMenu();
                break;
            default:
                Debug.Log("Out of range");
                break;
        }
    }

    private void MoveTo2nd()
    {
        LeanTween.moveLocalX(gameObject, 41f, 2f).setEaseInOutExpo();
    }

    private void MoveTo3rd()
    {
        cameraSizeTarget = 9f;
        LeanTween.moveLocalX(gameObject, 72.2f, 2f).setEaseInOutExpo();
        LeanTween.moveLocalY(gameObject, 6.6f, 2f).setEaseInOutExpo();
    }

    private void MoveTo4th()
    {
        LeanTween.moveLocalY(gameObject, -6.8f, 1f).setEaseInOutExpo();
    }

    private void MoveTo5th()
    {
        cameraSizeTarget = 14f;
        LeanTween.moveLocalX(gameObject, 0f, 2f).setEaseInOutExpo();
        LeanTween.moveLocalY(gameObject, -27.1f, 2f).setEaseInOutExpo();
    }

    private void MoveTo6th()
    {
        cameraSizeTarget = 9f;
        LeanTween.moveLocalX(gameObject, 30.4f, 2f).setEaseInOutExpo();
        LeanTween.moveLocalY(gameObject, -20.2f, 2f).setEaseInOutExpo();
    }

    private void MoveTo7th()
    {
        LeanTween.moveLocalY(gameObject, -33.5f, 1f).setEaseInOutExpo();
    }

    private void MoveTo8th()
    {
        cameraSizeTarget = 14f;
        LeanTween.moveLocalX(gameObject, 59.7f, 2f).setEaseInOutExpo();
        LeanTween.moveLocalY(gameObject, -27.1f, 2f).setEaseInOutExpo();
    }

    private void MoveTo9th()
    {
        cameraSizeTarget = 35f;
        LeanTween.moveLocalX(gameObject, 30.7f, 2f).setEaseInOutExpo();
        LeanTween.moveLocalY(gameObject, -74.4f, 2f).setEaseInOutExpo();
    }

    private void WhiteScreenTransition()
    {
        whiteSquareTransition.SetActive(true);
    }

    private void LoadMainMenu()
    {
        _roomQueue.LoadRoom("WarehouseRoom1");
    }

}

