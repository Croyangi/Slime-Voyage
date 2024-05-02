using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int deathCount;
    public SerializableDictionary<string, bool> checkpointsReached;
    public SerializableDictionary<string, bool> newLocationsDiscovered;
    public SerializableDictionary<string, bool> areasCompleted;

    public SerializableDictionary<string, bool> warehouseDoorsUnlocked;

    public SerializableDictionary<string, float> recordSpeedrunTimes;
    public bool isSpeedrunModeOn;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    // Default Values
    public GameData() 
    { 
        deathCount = 0;

        checkpointsReached = new SerializableDictionary<string, bool>();
        newLocationsDiscovered = new SerializableDictionary<string, bool>();
        areasCompleted = new SerializableDictionary<string, bool>();

        warehouseDoorsUnlocked = new SerializableDictionary<string, bool>();

        recordSpeedrunTimes = new SerializableDictionary<string, float>();

        isSpeedrunModeOn = false;

        masterVolume = 1;
        musicVolume = 1;
        sfxVolume = 1;
    }
}