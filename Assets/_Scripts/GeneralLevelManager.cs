using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RoomQueue _roomQueue;
    [SerializeField] private float startingTransitionCutoff; // Determines how far along the transition is
    [SerializeField] private float startingTransitionTarget;
    [SerializeField] private float startingTransitionSpeed;
    [SerializeField] private Texture startingTransitionTexture;

    [SerializeField] private float endingTransitionCutoff; // Determines how far along the transition is
    [SerializeField] private float endingTransitionTarget;
    [SerializeField] private float endingTransitionSpeed;
    [SerializeField] private Texture endingTransitionTexture;

    [SerializeField] private Image transitionImage;
    [SerializeField] private Material transitionMaterial;

    [Header("Variables")]
    [SerializeField] public bool isEndedStartingTransition;
    [SerializeField] public bool isRoomComplete;
    [SerializeField] public bool isStartEndingTransition;
    [SerializeField] private string roomName;

    [Header("Settings")]
    [SerializeField] private bool isStartingTransitionDisabled;
    [SerializeField] private bool isEndingTransitionDisabled;

    private void Awake()
    {
        transitionMaterial = new Material(transitionImage.GetComponent<Image>().material);
        transitionImage.material = transitionMaterial;

        transitionImage.material.SetFloat("_Cutoff", startingTransitionCutoff);
        transitionImage.material.SetTexture("_WarehouseTransitionTexture", startingTransitionTexture);

        if (isStartingTransitionDisabled)
        {
            transitionImage.material.SetFloat("_Cutoff", endingTransitionCutoff);
            isStartEndingTransition = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isEndedStartingTransition) 
        {
            startingTransitionCutoff = transitionImage.material.GetFloat("_Cutoff");
            transitionImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(startingTransitionCutoff, startingTransitionTarget, startingTransitionSpeed * Time.deltaTime));

            if (startingTransitionTarget == startingTransitionCutoff)
            {
                isStartEndingTransition = true;
                isEndedStartingTransition = true;
            }
        }

        if (isStartEndingTransition && isRoomComplete)
        {
            transitionImage.material.SetFloat("_Cutoff", endingTransitionCutoff);
            isStartEndingTransition = false;
        }

        if (isRoomComplete)
        {
            transitionImage.material.SetTexture("_WarehouseTransitionTexture", endingTransitionTexture);
            endingTransitionCutoff = transitionImage.material.GetFloat("_Cutoff");
            transitionImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(endingTransitionCutoff, endingTransitionTarget, endingTransitionSpeed * Time.deltaTime));
        }

        if ((endingTransitionTarget == endingTransitionCutoff) || isEndingTransitionDisabled)
        {
            NextLevel(roomName);
        }
    }

    private void NextLevel(string roomName)
    {
        _roomQueue.LoadRoom(roomName);
    }
}
