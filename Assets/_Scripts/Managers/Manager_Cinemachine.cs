using Cinemachine;
using System.Collections;
using UnityEngine;

public class Manager_Cinemachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject currentCinemachine;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCam;

    [SerializeField] private CinemachineBasicMultiChannelPerlin channel;
    [SerializeField] private NoiseSettings cnp_2dScreenShake;

    [SerializeField] private float screenShakeTime;
    [SerializeField] private float screenShakeAmplitude;

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

    private IEnumerator SubscribeAfterAFrame()
    {
        yield return new WaitForFixedUpdate();
        Manager_PlayerState.instance.onPlayerMoldChanged += ChangeTarget;
    }

    private void ChangeTarget()
    {
        cinemachineVirtualCam = currentCinemachine.GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCam.m_Follow = Manager_PlayerState.instance.player.transform;
        channel = LoadCinemachineChannel(cinemachineVirtualCam);
    }

    public void OnChangeCinemachine(GameObject cinemachine)
    {
        // Cleanup
        cinemachineVirtualCam = currentCinemachine.GetComponent<CinemachineVirtualCamera>();
        channel = LoadCinemachineChannel(cinemachineVirtualCam);


        // Post Cleanup
        currentCinemachine = cinemachine;
        cinemachineVirtualCam = currentCinemachine.GetComponent<CinemachineVirtualCamera>();

        cinemachineVirtualCam.m_Follow = Manager_PlayerState.instance.player.transform;
        channel = LoadCinemachineChannel(cinemachineVirtualCam);

        // Reapply
        ApplyScreenShake(screenShakeTime, screenShakeAmplitude);
    }


    // Returns the channel
    private CinemachineBasicMultiChannelPerlin LoadCinemachineChannel(CinemachineVirtualCamera cinemachine)
    {
        if (cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() == null)
        {
            cinemachine.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        return cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ApplyScreenShake(float time, float amplitude)
    {
        screenShakeTime = time;
        screenShakeAmplitude = amplitude;

        channel.m_NoiseProfile = cnp_2dScreenShake;
        channel.m_AmplitudeGain = amplitude;
    }

    private void FixedUpdate()
    {
        // Screenshake
        if (screenShakeTime > 0f)
        {
            screenShakeTime -= Time.deltaTime;
        } else
        {
            screenShakeAmplitude = 0f;
            if (channel != null)
            {
                channel.m_AmplitudeGain = 0f;
            }
        }
    }
}
