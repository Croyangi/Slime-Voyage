using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_ResultsScreen : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private ScriptObj_AreaId _areaId;

    [SerializeField] private TextMeshProUGUI tmp_checkpointsReached;
    [SerializeField] private TextMeshProUGUI tmp_newLocationsDiscovered;

    [SerializeField] private GameObject speedrun;
    [SerializeField] private TextMeshProUGUI tmp_speedrunTime;
    [SerializeField] private TextMeshProUGUI tmp_speedrunRecord;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;
    [SerializeField] private string scene_loadingScreen;
    [SerializeField] private string scene_activeScene;

    public TimeSpan timeElapsed { get; private set; }

    private void Awake()
    {
        speedrun.SetActive(false);
        StartCoroutine(LoadLoadingScreen());
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_activeScene));
    }

    private IEnumerator LoadLoadingScreen()
    {
        // Transition screen
        if (Manager_LoadingScreen.instance != null)
        {
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_loadingScreen, LoadSceneMode.Additive);

            // Wait until the asynchronous scene loading is complete
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Finished");
            Manager_LoadingScreen.instance.OpenLoadingScreen();
        }
    }

    private void SetSpeedrunTimes(float current = 0f, float record = 0f)
    {
        speedrun.SetActive(true);

        TimeSpan speedrunTime = TimeSpan.FromSeconds(current);
        string time =         speedrunTime.Minutes.ToString("00") + ":" +
                              speedrunTime.Seconds.ToString("00") + "." +
                              speedrunTime.Milliseconds.ToString("000");
        tmp_speedrunTime.text = "Speedrun Time: " + time;

        TimeSpan recordTime = TimeSpan.FromSeconds(record);
        time =                recordTime.Minutes.ToString("00") + ":" +
                              recordTime.Seconds.ToString("00") + "." +
                              recordTime.Milliseconds.ToString("000");
        tmp_speedrunRecord.text = "Current Record Time: " + time;
    }

    private void SetCheckpointsReached(int amount, int totalAmount)
    {
        tmp_checkpointsReached.text = "Checkpoints Reached: " + amount + "/" + totalAmount;
    }

    private void SetLocationsDiscovered(int amount, int totalAmount)
    {
        tmp_newLocationsDiscovered.text = "Locations Discovered: " + amount + "/" + totalAmount;
    }

    public void LoadData(GameData data)
    {
        // Speedrun Times
        float recordTime;
        data.recordSpeedrunTimes.TryGetValue(_areaId.name, out recordTime);

        if (data.isSpeedrunModeOn)
        {
            SetSpeedrunTimes(data.currentSpeedrunTime, recordTime);
        }
        //

        // Checkpoints
        string checkpointFieldName = _areaId.name + "_checkpointsReached";
        SerializableDictionary<string, bool> checkpointsReached = (SerializableDictionary<string, bool>)data.GetType().GetField(checkpointFieldName).GetValue(data);

        int checkpointCount = 0;
        foreach (bool value in checkpointsReached.Values) 
        { 
            if (value)
            {
                checkpointCount++;
            }
        }
        SetCheckpointsReached(checkpointCount, checkpointsReached.Count);
        //

        // Areas Discovered
        string locationFieldName = _areaId.name + "_newLocationsDiscovered";
        SerializableDictionary<string, bool> newLocationsDiscovered = (SerializableDictionary<string, bool>)data.GetType().GetField(locationFieldName).GetValue(data);

        int locationCount = 0;
        foreach (bool value in newLocationsDiscovered.Values)
        {
            if (value)
            {
                locationCount++;
            }
        }
        SetLocationsDiscovered(locationCount, newLocationsDiscovered.Count);
        //
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
