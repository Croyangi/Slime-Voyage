using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunkfish_Culler : MonoBehaviour, ICuller
{
    [SerializeField] private List<Chunkfish_State> states = new List<Chunkfish_State>();
    public bool isCulledActive;

    private void Start()
    {
        DeloadObjects();

        // Remove null states in prefab edge cases
        List<Chunkfish_State> newStates = new List<Chunkfish_State>();
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
    }

    public void DeloadObjects()
    {
        isCulledActive = false;
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
