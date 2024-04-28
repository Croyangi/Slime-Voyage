using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Manager_DialogueHandler;

public class Manager_Jukebox : MonoBehaviour
{
    [Serializable]
    public class MusicSet
    {
        public AudioClip music;
        public string name;
        public float volume;
    }
    [SerializeField] private List<MusicSet> musicSet;
    [SerializeField] private AudioSource audioSource_song;

    [SerializeField] private int musicSetIndex = 0;
    [SerializeField] private float initialVolume;


    public static Manager_Jukebox instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Jukebox Manager in the scene.");
        }

        instance = this;

        SetupJukebox();
    }

    private void SetupJukebox()
    {
        audioSource_song.clip = musicSet[musicSetIndex].music;
        audioSource_song.volume = musicSet[musicSetIndex].volume;

        initialVolume = audioSource_song.volume;
        audioSource_song.ignoreListenerPause = true;
    }

    public void PlayJukebox()
    {
        SetupJukebox();
        audioSource_song.Play();
        if (Manager_PauseMenu.instance != null)
        {
            Manager_PauseMenu.instance.ChangeSongText(musicSet[musicSetIndex].name);
        }
    }

    // Volume is 0 - 1
    public void SetVolume(float volume)
    {
        audioSource_song.volume = initialVolume * volume;
    }
}
