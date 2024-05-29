using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_BasementElevatorIntro : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject elevatorRotatingRedLights;

    private void Awake()
    {
        LeanTween.rotateAround(elevatorRotatingRedLights, Vector3.up, 360, 2f).setLoopClamp();
    }
}
