using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Manager_AudioMixer : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] public AudioMixerGroup mixer_master;
    [SerializeField] public AudioMixerGroup mixer_sfx;
    [SerializeField] public AudioMixerGroup mixer_music;

    [SerializeField] private GameObject slider_master;
    [SerializeField] private GameObject slider_music;
    [SerializeField] private GameObject slider_sfx;

    public static Manager_AudioMixer instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }

    public void LoadData(GameData data)
    {
        SetMasterVolume(data.masterVolume);
        SetMusicVolume(data.musicVolume);
        SetSFXVolume(data.sfxVolume);
    }

    public void SaveData(ref GameData data)
    {
        audioMixer.GetFloat("masterVolume", out float masterVolume);
        data.masterVolume = Mathf.Pow(10f, masterVolume / 20f);
        audioMixer.GetFloat("musicVolume", out float musicVolume);
        data.musicVolume = Mathf.Pow(10f, musicVolume / 20f);
        audioMixer.GetFloat("sfxVolume", out float sfxVolume);
        data.sfxVolume = Mathf.Pow(10f, sfxVolume / 20f);
    }


    public void SetMasterVolume(float level)
    {
        if (slider_master != null) { slider_master.GetComponent<Slider>().value = level; }
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        if (slider_music != null) { slider_music.GetComponent<Slider>().value = level; }
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        if (slider_sfx != null) { slider_sfx.GetComponent<Slider>().value = level; }
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
}
