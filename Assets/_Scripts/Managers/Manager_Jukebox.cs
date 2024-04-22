using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Jukebox : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip_breakingProtocol;
    [SerializeField] private AudioSource audioSource_song;
    [SerializeField] private float initialVolume;


    public static Manager_Jukebox instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Jukebox Manager in the scene.");
        }

        instance = this;

        initialVolume = audioSource_song.volume;
        audioSource_song.ignoreListenerPause = true;
    }

    public void PlayBreakingProtocol()
    {
        audioSource_song.Play();
    }

    // Volume is 0 - 1
    public void SetVolume(float volume)
    {
        audioSource_song.volume = initialVolume * volume;
    }
}
