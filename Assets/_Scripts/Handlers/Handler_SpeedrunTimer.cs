using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Handler_SpeedrunTimer : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private bool isTimerActive = false;
    [SerializeField] private bool isSpeedrunFinished = false;
    [SerializeField] private float currentTime;
    [SerializeField] private float recordTime;

    [Header("UI")]
    [SerializeField] private GameObject ui_currentTime;
    [SerializeField] private GameObject ui_recordTime;
    [SerializeField] private TextMeshProUGUI tmp_currentTime;
    [SerializeField] private TextMeshProUGUI tmp_recordTime;

    public TimeSpan timeElapsed { get; private set; }

    private void Start()
    {
        currentTime = 0f;
        timeElapsed = TimeSpan.FromSeconds(currentTime);
        tmp_currentTime.text = timeElapsed.Minutes.ToString("00") + ":" +
                               timeElapsed.Seconds.ToString("00") + "." +
                               timeElapsed.Milliseconds.ToString("000");
    }

    private void Update()
    {
        if (isTimerActive)
        {
            currentTime += Time.deltaTime;

            timeElapsed = TimeSpan.FromSeconds(currentTime);
            tmp_currentTime.text = timeElapsed.Minutes.ToString("00") + ":" +
                                   timeElapsed.Seconds.ToString("00") + "." +
                                   timeElapsed.Milliseconds.ToString("000");
        }
    }

    public void SetSpeedrunTimer(bool isActive)
    {
        isTimerActive = isActive;
    }

    public void EndSpeedrunTimer()
    {
        isTimerActive = false;
        isSpeedrunFinished = true;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        if (data.recordSpeedrunTime != 0f)
        {
            recordTime = data.recordSpeedrunTime;

            TimeSpan speedrunRecordTime = TimeSpan.FromSeconds(recordTime);
            tmp_recordTime.text = speedrunRecordTime.Minutes.ToString("00") + ":" +
                                  speedrunRecordTime.Seconds.ToString("00") + "." +
                                  speedrunRecordTime.Milliseconds.ToString("000");
        }

    }

    public void SaveData(ref GameData data)
    {
        if (currentTime > recordTime && isSpeedrunFinished)
        {
            data.recordSpeedrunTime = currentTime;
        }
    }
}
