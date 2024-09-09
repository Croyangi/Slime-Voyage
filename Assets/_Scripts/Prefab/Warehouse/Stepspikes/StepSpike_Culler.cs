using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpike_Culler : MonoBehaviour, ICuller
{
    [SerializeField] private List<StepSpike_State> states = new List<StepSpike_State>();
    public bool isCulledActive;

    private void Start()
    {
        DeloadObjects();

        // Remove null states in prefab edge cases
        List<StepSpike_State> newStates = new List<StepSpike_State>();
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
