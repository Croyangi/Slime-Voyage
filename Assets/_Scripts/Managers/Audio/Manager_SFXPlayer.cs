using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Manager_SFXPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxObject;

    public static Manager_SFXPlayer instance { get; private set; }
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }

    private int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }


    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f, bool isLooping = false, AudioMixerGroup mixerGroup = null, bool isPitchShifted = false, float pitchShift = 0f, float spatialBlend = 0, float minDistance = 1, float maxDistance = 500, float spread = 0, bool isUnaffectedByTime = false)
    {
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = isLooping;

        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.spatialBlend = spatialBlend;
        audioSource.spread = spread;

        audioSource.ignoreListenerPause = isUnaffectedByTime;

        // Pitch Shift
        float pitch;
        if (isPitchShifted)
        {
            pitch = (RandomSign() * Random.Range(0, pitchShift));
        }
        else
        {
            pitch = pitchShift;
        }
        audioSource.pitch += pitch;

        // Mixing
        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }

        audioSource.Play();

        // Looping
        if (isLooping == false)
        {
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength / audioSource.pitch);
        }
    }

    public void PlayRandomSFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f, bool isLooping = false, AudioMixerGroup mixerGroup = null, bool isPitchShifted = false, float pitchShift = 0f, float spatialBlend = 0, float minDistance = 1, float maxDistance = 500, float spread = 0, bool isUnaffectedByTime = false)
    {
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        audioSource.volume = volume;
        audioSource.loop = isLooping;

        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.spatialBlend = spatialBlend;
        audioSource.spread = spread;

        audioSource.ignoreListenerPause = isUnaffectedByTime;

        // Pitch Shift
        if (isPitchShifted)
        {
            float pitch = audioSource.pitch + (RandomSign() * Random.Range(0, pitchShift));
            audioSource.pitch = pitch;
        }

        // Mixing
        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }

        audioSource.Play();

        // Looping
        if (isLooping == false)
        {
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength / audioSource.pitch);
        }
    }
}
