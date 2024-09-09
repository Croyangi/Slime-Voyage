using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralCullerCommunicator : MonoBehaviour
{
    public virtual void FixedUpdateState()
    {
        // Default (optional) implementation, can be left empty
    }

    public virtual void OnLoad()
    {
        // Default (optional) implementation, can be left empty
    }
    public virtual void OnCull()
    {
        // Default (optional) implementation, can be left empty
    }
}
