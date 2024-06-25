using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevTool_FPS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float minFPS = Mathf.Infinity;
    [SerializeField] private float maxFPS = Mathf.NegativeInfinity;
    [SerializeField] private float currentFPS;
    [SerializeField] private TextMeshProUGUI tmp_minFPS;
    [SerializeField] private TextMeshProUGUI tmp_maxFPS;
    [SerializeField] private TextMeshProUGUI tmp_currentFPS;

    private void OnEnable()
    {
        currentFPS = (1f / Time.deltaTime);
        minFPS = Mathf.Infinity;
        maxFPS = Mathf.NegativeInfinity;
        StopAllCoroutines();
        StartCoroutine(WarmUpFPS());
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayFPS());
        } else
        {
            StopAllCoroutines();
        }
    }

    public void RefreshMinMaxFPS()
    {
        minFPS = Mathf.Infinity;
        maxFPS = Mathf.NegativeInfinity;
    }

    private void Update()
    {
        currentFPS = (1f / Time.deltaTime);
    }

    private IEnumerator WarmUpFPS()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(DisplayFPS());
    }

    private IEnumerator DisplayFPS()
    {
        tmp_currentFPS.text = currentFPS.ToString("F0");

        if (currentFPS < minFPS)
        {
            minFPS = currentFPS;
            tmp_minFPS.text = minFPS.ToString("F0");
        }

        if (currentFPS > maxFPS)
        {
            maxFPS = currentFPS;
            tmp_maxFPS.text = maxFPS.ToString("F0");
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(DisplayFPS());
    }
}
