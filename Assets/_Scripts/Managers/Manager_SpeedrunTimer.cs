using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_SpeedrunTimer : MonoBehaviour, IDataPersistence
{
    [Header("References")]
    [SerializeField] private bool isTimerActive = false;
    [SerializeField] private bool isSpeedrunFinished = false;

    [SerializeField] private float currentTime;
    [SerializeField] private float recordTime;

    [SerializeField] private ScriptObj_AreaId _areaId;

    [Header("UI")]
    [SerializeField] private GameObject ui_currentTime;
    [SerializeField] private GameObject ui_recordTime;
    [SerializeField] private TextMeshProUGUI tmp_currentTime;
    [SerializeField] private TextMeshProUGUI tmp_recordTime;

    public static Manager_SpeedrunTimer instance { get; private set; }

    public TimeSpan timeElapsed { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Speedrun Manager in the scene.");
        }
        instance = this;

        PrepareOpenSpeedrunTimer();


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

    [ContextMenu("Close Speedrun Timer")]
    private void CloseSpeedrunTimer()
    {
        LeanTween.moveX(ui_currentTime.GetComponent<RectTransform>(), -500f, 2f).setEaseInOutBack().setDelay(3f);
        LeanTween.moveX(ui_recordTime.GetComponent<RectTransform>(), -527f, 2f).setEaseInOutBack().setDelay(3.3f);
    }

    [ContextMenu("Open Speedrun Timer")]
    public void OpenSpeedrunTimer()
    {
        LeanTween.moveX(ui_currentTime.GetComponent<RectTransform>(), 0f, 2f).setEaseInOutBack().setDelay(2f);
        LeanTween.moveX(ui_recordTime.GetComponent<RectTransform>(), -27f, 2f).setEaseInOutBack().setDelay(2.3f);
    }

    [ContextMenu("Prepare Open Speedrun Timer")]
    private void PrepareOpenSpeedrunTimer()
    {
        LeanTween.moveX(ui_currentTime.GetComponent<RectTransform>(), -500f, 0f).setEaseInOutBack();
        LeanTween.moveX(ui_recordTime.GetComponent<RectTransform>(), -527f, 0f).setEaseInOutBack();
    }


    public void StartSpeedrunTimer()
    {
        SetSpeedrunTimer(true);
    }

    private void SetSpeedrunTimer(bool isActive)
    {
        isTimerActive = isActive;
    }

    public void EndSpeedrunTimer()
    {
        isTimerActive = false;
        isSpeedrunFinished = true;
        DataPersistenceManager.instance.SaveGame();
        CloseSpeedrunTimer();
    }


    public void LoadData(GameData data)
    {
        data.recordSpeedrunTimes.TryGetValue(_areaId.name, out recordTime);

        if (recordTime != 0f)
        {
            TimeSpan speedrunRecordTime = TimeSpan.FromSeconds(recordTime);
            tmp_recordTime.text = speedrunRecordTime.Minutes.ToString("00") + ":" +
                                  speedrunRecordTime.Seconds.ToString("00") + "." +
                                  speedrunRecordTime.Milliseconds.ToString("000");
        }

    }

    public void SaveData(ref GameData data)
    {
        data.currentSpeedrunTime = currentTime;

        if (currentTime != 0f)
        {
            // In case no record was set yet
            float tempRecordTime = recordTime;
            if (tempRecordTime == 0f)
            {
                tempRecordTime = Mathf.Infinity;
            }

            // Lots of additional checks just to discourage speedrun cheating
            if (currentTime < tempRecordTime && isSpeedrunFinished)
            {
                if (data.recordSpeedrunTimes.ContainsKey(_areaId.name))
                {
                    data.recordSpeedrunTimes.Remove(_areaId.name);
                }
                data.recordSpeedrunTimes.Add(_areaId.name, currentTime);
            }
        }
    }
}
