using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Chunkfish_Main : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject chunkfish;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float _amplitude = 0;
    [SerializeField] private float _frequency = 1;
    [SerializeField] private float _amplitudeRotate = 0;
    [SerializeField] private float _frequencyRotate = 1;

    private void FixedUpdate()
    {
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        float rotateZ = Mathf.Sin(Time.time * _frequencyRotate) * _amplitudeRotate;
        chunkfish.transform.position = new Vector2(chunkfish.transform.position.x, chunkfish.transform.position.y + y);
        chunkfish.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
    }
}
