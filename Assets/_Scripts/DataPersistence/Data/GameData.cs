using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class AreaSet
{
    public string areaId = string.Empty;
    public SerializableDictionary<string, bool> checkpointsReached;
    public SerializableDictionary<string, bool> locationsDiscovered;
    public SerializableDictionary<string, bool> collectiblesCollected;
}

public class GameData
{
    public bool hasPlayedGameBefore;

    public int deathCount;

    public List<AreaSet> areaSets = new List<AreaSet>();

    public SerializableDictionary<string, bool> warehouseDoorsUnlocked;

    public SerializableDictionary<string, bool> areasCompleted;

    public string resultsScreenId;

    public float currentSpeedrunTime;
    public SerializableDictionary<string, float> recordSpeedrunTimes;
    public bool isSpeedrunModeOn = false;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;


    // Search function within areaSets
    public AreaSet SearchAreaWithId(string id)
    {
        foreach (AreaSet set in areaSets)
        {
            if (set.areaId == id)
            {
                return set;
            }
        }
        Debug.Log("No area set with that id found");
        return null;
    }

    // Default Values
    public GameData() 
    { 
        deathCount = 0;

        areaSets = new List<AreaSet>();

        // Initiate areas
        InitiateAreas();

        warehouseDoorsUnlocked = new SerializableDictionary<string, bool>();

        areasCompleted = new SerializableDictionary<string, bool>();

        resultsScreenId = string.Empty;

        currentSpeedrunTime = 0f;
        recordSpeedrunTimes = new SerializableDictionary<string, float>();

        isSpeedrunModeOn = false;

        masterVolume = 1;
        musicVolume = 1;
        sfxVolume = 1;
    }

    public void InitiateAreas()
    {
        // Instantiate areas
        AreaSet warehouse = new AreaSet();
        warehouse.areaId = "warehouse";
        areaSets.Add(warehouse);

        AreaSet warehouseSwapeeMode = new AreaSet();
        warehouseSwapeeMode.areaId = "warehouseSwapeeMode";
        areaSets.Add(warehouseSwapeeMode);

        AreaSet basement = new AreaSet();
        basement.areaId = "basement";
        areaSets.Add(basement);
    }
}