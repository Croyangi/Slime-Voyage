using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool_FollowPlayerCinemachine : MonoBehaviour
{
    [SerializeField] private GameObject cinemachine;
    [SerializeField] private CinemachineVirtualCamera cvc;


    private void Awake()
    {
        cvc = cinemachine.GetComponent<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {
        if (cvc.m_Follow == null)
        {
            cvc.m_Follow = Manager_PlayerState.instance.player.transform;
        }
    }
}
