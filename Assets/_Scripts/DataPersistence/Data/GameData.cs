using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int deathCount;
    public SerializableDictionary<string, bool> checkpointsReached;
    public float recordSpeedrunTime;
    public bool isSpeedrunModeOn;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    // Default Values
    public GameData() 
    { 
        deathCount = 0;
        checkpointsReached = new SerializableDictionary<string, bool>();
        recordSpeedrunTime = 0f;
        isSpeedrunModeOn = false;

        masterVolume = 1;
        musicVolume = 1;
        sfxVolume = 1;
    }
}