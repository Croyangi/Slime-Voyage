using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwivel : MonoBehaviour
{
    [Header("Camera Work")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 cameraRotationOffset;
    [SerializeField] private float slerpScale;
    [SerializeField] private Vector2 parallaxOriginPoint;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 parallaxScale;

    private void Awake()
    {
        parallaxOriginPoint.x = Screen.width / 2;
        parallaxOriginPoint.y = Screen.height / 2;
        cameraRotationOffset = _camera.transform.localEulerAngles;
    }

    private void FixedUpdate()
    {
        parallaxOriginPoint.x = Screen.width / 2;
        parallaxOriginPoint.y = Screen.height / 2;
        ScreenFollowMouseUpdate();
    }

    private void ScreenFollowMouseUpdate()
    {
        GetParallax();

        float rotationX = offset.y * parallaxScale.x * -1;
        float rotationY = offset.x * parallaxScale.y;
        Vector3 parallaxRotation = new Vector3(rotationX + cameraRotationOffset.x, rotationY + cameraRotationOffset.y, cameraRotationOffset.z);

        Quaternion desiredRotation = Quaternion.Slerp(Quaternion.Euler(parallaxRotation), _camera.transform.rotation, slerpScale);

        _camera.transform.rotation = desiredRotation;
    }

    private void GetParallax()
    {
        Vector3 mousePosition = Input.mousePosition;
        offset = GetOffsetFromCenterScreen(parallaxOriginPoint, mousePosition);
    }

    private Vector2 GetOffsetFromCenterScreen(Vector2 pos1, Vector2 pos2)
    {
        float distanceX = pos2.x - pos1.x;
        float distanceY = pos2.y - pos1.y;
        Vector2 distance = new Vector2(distanceX, distanceY);

        return distance;
    }
}
