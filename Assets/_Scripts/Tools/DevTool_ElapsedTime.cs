using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevTool_ElapsedTime : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float time;
    [SerializeField] private TextMeshProUGUI tmp_time;

    private void FixedUpdate()
    {
        string timeText = System.TimeSpan.FromSeconds(Time.realtimeSinceStartup).ToString("hh':'mm':'ss");
        tmp_time.text = timeText;
    }
}
