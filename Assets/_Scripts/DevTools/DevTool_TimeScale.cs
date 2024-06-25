using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevTool_TimeScale : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string timeScaleText = "Time Scale: ";
    [SerializeField] private TextMeshProUGUI _textMeshProTimeScaleText;
    [SerializeField] private float currentTimeScale;

    public void OnAddTimeScale()
    {
        currentTimeScale += 0.1f;
        OnTimeScaleChange();
    }

    public void OnSubtractTimeScale()
    {
        currentTimeScale -= 0.1f;
        OnTimeScaleChange();
    }

    private void OnTimeScaleChange()
    {
        currentTimeScale = Mathf.Clamp(currentTimeScale, 0, 999);
        Time.timeScale = currentTimeScale;

        double roundedNumber = System.Math.Round(currentTimeScale, 1);
        _textMeshProTimeScaleText.text = timeScaleText + roundedNumber;
    }
}
