using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int deathCount;

    //public SerializableDictionary<string, bool> checkpointsReached;
    //public SerializableDictionary<string, bool> newLocationsDiscovered;

    public SerializableDictionary<string, bool> warehouse_checkpointsReached;
    public SerializableDictionary<string, bool> warehouse_newLocationsDiscovered;
    public SerializableDictionary<string, bool> warehouseDoorsUnlocked;

    public SerializableDictionary<string, bool> areasCompleted;

    public float currentSpeedrunTime;
    public SerializableDictionary<string, float> recordSpeedrunTimes;
    public bool isSpeedrunModeOn;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    // Default Values
    public GameData() 
    { 
        deathCount = 0;

        warehouse_checkpointsReached = new SerializableDictionary<string, bool>();
        warehouse_newLocationsDiscovered = new SerializableDictionary<string, bool>();
        warehouseDoorsUnlocked = new SerializableDictionary<string, bool>();

        areasCompleted = new SerializableDictionary<string, bool>();

        currentSpeedrunTime = 0f;
        recordSpeedrunTimes = new SerializableDictionary<string, float>();

        isSpeedrunModeOn = false;

        masterVolume = 1;
        musicVolume = 1;
        sfxVolume = 1;
    }
}