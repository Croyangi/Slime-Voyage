using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_TargetFrameRate : MonoBehaviour
{
    [SerializeField] private bool isSet;

    public void SetTargetFrameRate()
    {
        if (Application.targetFrameRate == 60)
        {
            SetDefaultFrameRate();
        } else
        {
            Application.targetFrameRate = 60;
        }
    }

    public void SetDefaultFrameRate()
    {
        Application.targetFrameRate = -1;
    }
}
