using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject chunkfish;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float _amplitude = 0.01f;
    [SerializeField] private float _frequency = 3;
    [SerializeField] private float time;

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time * _frequency) * _amplitude;
        chunkfish.transform.position = new Vector2(chunkfish.transform.position.x, chunkfish.transform.position.y + y);
    }
}
