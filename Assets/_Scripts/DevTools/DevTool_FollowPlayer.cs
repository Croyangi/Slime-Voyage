using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject cinemachine;
    [SerializeField] private CinemachineVirtualCamera cvc;

    [Header("Camera References")]
    [SerializeField] private float cameraSize;

    private void Awake()
    {
        cvc = cinemachine.GetComponent<CinemachineVirtualCamera>();
        cameraSize = cvc.m_Lens.OrthographicSize;
    }

    public void ToggleFollowPlayer()
    {
        cinemachine.SetActive(!cinemachine.activeSelf);
        cvc.m_Follow = Manager_PlayerState.instance.player.transform;
    }

    public void OnAddCameraSize()
    {
        cameraSize += 0.5f;
        ResetCameraSettings();
    }

    public void OnSubtractCameraSize()
    {
        cameraSize -= 0.5f;
        ResetCameraSettings();
    }

    private void ResetCameraSettings()
    {
        cameraSize = Mathf.Clamp(cameraSize, 1, 9999);
        cvc.m_Lens.OrthographicSize = cameraSize;
    }
}
