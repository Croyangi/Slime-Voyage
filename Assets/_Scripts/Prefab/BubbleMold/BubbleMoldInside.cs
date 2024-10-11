using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMoldInside : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bubbleMoldInside;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float _amplitude = 0;
    [SerializeField] private float _frequency = 1;
    [SerializeField] private float _amplitudeRotate = 0;
    [SerializeField] private float _frequencyRotate = 1;

    private void FixedUpdate()
    {
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        float rotateZ = Mathf.Sin(Time.time * _frequencyRotate) * _amplitudeRotate;
        bubbleMoldInside.transform.position = new Vector2(bubbleMoldInside.transform.position.x, bubbleMoldInside.transform.position.y + y);
        bubbleMoldInside.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
    }
}
