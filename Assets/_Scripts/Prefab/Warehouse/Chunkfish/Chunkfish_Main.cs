using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Chunkfish_Main : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject chunkfish;
    [SerializeField] private Vector2 initialPos;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float _amplitude = 0;
    [SerializeField] private float _frequency = 1;
    [SerializeField] private float _amplitudeRotate = 0;
    [SerializeField] private float _frequencyRotate = 1;
    [SerializeField] private float time;

    private void OnEnable()
    {
        initialPos = chunkfish.transform.position;
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time * _frequency) * _amplitude;
        float rotateZ = Mathf.Sin(time * _frequencyRotate) * _amplitudeRotate;

        // Halving amplitude and subtracting cause we want to center amplitude
        chunkfish.transform.position = new Vector2(initialPos.x, initialPos.y + y - (_amplitude / 2f));
        chunkfish.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
    }
}
