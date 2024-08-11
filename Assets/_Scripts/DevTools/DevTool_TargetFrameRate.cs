using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_TargetFrameRate : MonoBehaviour
{
    [SerializeField] private bool isSet;
    [SerializeField] private int frameRate;

    public void SetTargetFrameRate()
    {
        if (isSet)
        {
            isSet = false;
            SetDefaultFrameRate();
        } else
        {
            isSet = true;
            Application.targetFrameRate = frameRate;
        }
    }

    public void SetDefaultFrameRate()
    {
        Application.targetFrameRate = -1;
    }
}
