using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader_WorldSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject miniDiorama;
    [SerializeField] private float miniDiorama_rotationSpeed;

    [SerializeField] private float miniDiorama_frequency;
    [SerializeField] private float miniDiorama_amplitude;

    private void FixedUpdate()
    {
        float y = Mathf.Sin(Time.time * miniDiorama_frequency) * miniDiorama_amplitude;
        miniDiorama.transform.position = new Vector3(miniDiorama.transform.position.x, miniDiorama.transform.position.y + y, miniDiorama.transform.position.z);

        miniDiorama.transform.Rotate(new Vector3(0, miniDiorama_rotationSpeed, 0));
    }
}
