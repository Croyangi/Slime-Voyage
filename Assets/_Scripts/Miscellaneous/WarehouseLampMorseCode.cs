using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WarehouseLampMorseCode : MonoBehaviour
{
    [SerializeField] private GameObject flickeringLight;
    [SerializeField] private string morseCode;

    private void Awake()
    {
        flickeringLight.SetActive(false);
        StartCoroutine(MorseCodeFlicker());
    }

    private void OnEnable()
    {
        flickeringLight.SetActive(false);
        StartCoroutine(MorseCodeFlicker());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MorseCodeFlicker()
    {
        foreach (char letter in morseCode) 
        { 
            // Dot
            if (letter == '.')
            {
                flickeringLight.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                flickeringLight.SetActive(false);
            }

            // Dash
            if (letter == '-')
            {
                flickeringLight.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                flickeringLight.SetActive(false);
            }

            // Space
            if (letter == ' ')
            {
                yield return new WaitForSeconds(0.5f);
            } else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(MorseCodeFlicker());
    }
}
