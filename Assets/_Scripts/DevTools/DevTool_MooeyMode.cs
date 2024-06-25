using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DevTool_MooeyMode : MonoBehaviour
{
    [SerializeField] private GameObject globalVolume;

    public void TogglePostProcessing()
    {
        globalVolume.SetActive(!globalVolume.activeSelf);
    }
}
