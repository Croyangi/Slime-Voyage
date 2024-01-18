using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WarehouseMusic
{
    public AudioSource source;
    public List<AudioClip> clips;
    public AudioClip currentAudioClip;
    public List<AudioClip> queue;
}
