using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GeneralCuller : MonoBehaviour, ICuller
{
    [SerializeField] private List<GeneralCullerCommunicator> states = new List<GeneralCullerCommunicator>();
    public bool isCulledActive;

    private void Start()
    {
        DeloadObjects();

        // Remove null states in prefab edge cases
        List<GeneralCullerCommunicator> newStates = new List<GeneralCullerCommunicator>();
        foreach (var state in states)
        {
            if (state != null)
            {
                newStates.Add(state);
            }
        }
        states.Clear();
        states.AddRange(newStates);
    }

    public void LoadObjects()
    {
        isCulledActive = true;
        foreach (var state in states)
        {
            state.OnLoad();
        }
    }

    public void DeloadObjects()
    {
        isCulledActive = false;
        foreach (var state in states)
        {
            state.OnCull();
        }
    }

    private void FixedUpdate()
    {
        if (isCulledActive)
        {
            foreach (var state in states)
            {
                state.FixedUpdateState();
            }
        }
    }
}
