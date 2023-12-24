using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraHandler cameraHandler;

    [Header("Variables")]
    [SerializeField] private bool followX;
    [SerializeField] private bool followY;
    [SerializeField] private float cameraX;
    [SerializeField] private float cameraY;
    [SerializeField] private float playerOffsetX;
    [SerializeField] private float playerOffsetY;

    [SerializeField] private float[] clampX;
    [SerializeField] private float[] clampY;
    [SerializeField] private bool isClampX;
    [SerializeField] private bool isClampY;

    private void Start()
    {
        cameraHandler = GameObject.FindWithTag("MainCamera").GetComponent<CameraHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            SetData();
        }
    }

    private void SetData()
    {
        cameraHandler.followX = followX;
        cameraHandler.followY = followY;
        cameraHandler.cameraX = cameraX;
        cameraHandler.cameraY = cameraY;
        cameraHandler.playerOffsetX = playerOffsetX;
        cameraHandler.playerOffsetY = playerOffsetY;
        cameraHandler.clampX = clampX;
        cameraHandler.clampY = clampY;
        cameraHandler.isClampX = isClampX;
        cameraHandler.isClampY = isClampY;
    }
}
