using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_OminousEmployerIntroCamera : MonoBehaviour
{
    [Header("Camera Work")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 cameraRotationOffset;
    [SerializeField] private float slerpScale;
    [SerializeField] private Vector2 parallaxOriginPoint;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 parallaxScale;

    [SerializeField] private Vector2 lastNodDirection;
    [SerializeField] private int nodYesDetection;
    [SerializeField] private int nodNoDetection;

    [SerializeField] private Handler_OminousEmployerIntro _ominousEmployerIntro;

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
        ScreenFollowMouse();
        GetParallax();
    }

    private void ScreenFollowMouse()
    {
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

    public void InitiateNoddingDetect()
    {
        nodYesDetection = 0;
        nodNoDetection = 0;

        StopAllCoroutines();
        StartCoroutine(NoddingDetectCheck());
    }

    private IEnumerator NoddingDetectCheck(float time = 0)
    {
        NoddingDetect();

        if (nodYesDetection >= 4)
        {
            _ominousEmployerIntro.NoddingCallback?.Invoke(true);
            yield break;
        } else if (nodNoDetection >= 4)
        {
            _ominousEmployerIntro.NoddingCallback?.Invoke(false);
            yield break;
        }
        
        if (time > 8f)
        {
            _ominousEmployerIntro.NoddingCallback?.Invoke(true);
            Debug.Log("Waiting too long, proceeding.");
            yield break;
        }
        else
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
            StartCoroutine(NoddingDetectCheck(time));
        }
    }

    // Detects if you are nodding yes or no
    private void NoddingDetect()
    {
        // Detect if you went at least xx% nodding yes
        if (Input.mousePosition.y > Screen.height * 0.60f && lastNodDirection.y != 1)
        {
            lastNodDirection = Vector2.up;
            nodYesDetection++;
            nodNoDetection = 0;
        } else if (Input.mousePosition.y < Screen.height * 0.40f && lastNodDirection.y != -1)
        {
            lastNodDirection = -Vector2.up;
            nodYesDetection++;
            nodNoDetection = 0;
        }

        // Detect if you went at least 80% nodding no
        if (Input.mousePosition.x > Screen.width * 0.60f && lastNodDirection.x != 1)
        {
            lastNodDirection = Vector2.right;
            nodNoDetection++;
            nodYesDetection = 0;
        }
        else if (Input.mousePosition.x < Screen.width * 0.40f && lastNodDirection.x != -1)
        {
            lastNodDirection = -Vector2.right;
            nodNoDetection++;
            nodYesDetection = 0;
        }

    }
}
