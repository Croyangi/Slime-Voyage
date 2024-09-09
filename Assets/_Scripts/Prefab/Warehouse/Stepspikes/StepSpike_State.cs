using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StepSpike_State : MonoBehaviour
{
    public abstract void FixedUpdateState();

    public virtual void OnLoad()
    {
        // Default (optional) implementation, can be left empty
    }
    public virtual void OnCull()
    {
        // Default (optional) implementation, can be left empty
    }
}
