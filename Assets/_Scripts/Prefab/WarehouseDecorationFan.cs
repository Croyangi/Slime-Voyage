using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseDecorationFan : MonoBehaviour
{
    [SerializeField] private GameObject fan;
    [SerializeField] private float rotationSpeed;

    private void FixedUpdate()
    {
        fan.transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
