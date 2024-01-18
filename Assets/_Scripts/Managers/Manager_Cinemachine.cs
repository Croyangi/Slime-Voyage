using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Cinemachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject currentCinemachine;

    public static Manager_Cinemachine instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Cinemachine Manager in the scene.");
        }
        instance = this;

        StopCoroutine(SubscribeAfterAFrame());
        StartCoroutine(SubscribeAfterAFrame());
    }

    private void ChangeTarget()
    {
        currentCinemachine.GetComponent<CinemachineVirtualCamera>().m_Follow = Manager_PlayerState.instance.player.transform;
    }

    public void OnChangeCinemachine(GameObject cinemachine)
    {
        currentCinemachine = cinemachine;
        currentCinemachine.GetComponent<CinemachineVirtualCamera>().m_Follow = Manager_PlayerState.instance.player.transform;
    }

    private IEnumerator SubscribeAfterAFrame()
    {
        yield return new WaitForFixedUpdate();
        Manager_PlayerState.instance.onPlayerMoldChanged += ChangeTarget;
    }
}
