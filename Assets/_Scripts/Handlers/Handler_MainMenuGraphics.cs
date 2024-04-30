using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_MainMenuGraphics : MonoBehaviour
{
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject logo;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private Vector2 slime_amplitude;
    [SerializeField] private Vector2 slime_frequency;
    [SerializeField] private Vector2 logo_amplitude;
    [SerializeField] private Vector2 logo_frequency;

    [SerializeField] private float elapsedTime;

    private void Awake()
    {
        // Instantiate initial variables to base randomness off
        //initAmplitude = amplitude;
        //initFrequency = frequency;

        //StartCoroutine(RandomizeOscillatingEffect());
    }

    private void FixedUpdate()
    {
        // Oscillates in a circular motion
        elapsedTime += Time.deltaTime;

        float slimeX = Mathf.Cos(elapsedTime * slime_frequency.x) * slime_amplitude.x;
        float slimeY = Mathf.Sin(elapsedTime * slime_frequency.y) * slime_amplitude.y;

        slime.transform.position = new Vector2(slime.transform.position.x + slimeX, slime.transform.position.y + slimeY);

        float logoX = Mathf.Cos(elapsedTime * logo_frequency.x) * logo_amplitude.x;
        float logoY = Mathf.Sin(elapsedTime * logo_frequency.y) * logo_amplitude.y;

        logo.transform.position = new Vector2(logo.transform.position.x + logoX, logo.transform.position.y + logoY);
    }

    // Randomizes the circular effect, to be less... circular
    //private IEnumerator RandomizeOscillatingEffect()
    //{
    //    yield return new WaitForSeconds(Random.Range(3f, 5f));

    //    Vector2 newAmplitude = new Vector2(initAmplitude.x + Random.Range(-initAmplitude.x, initAmplitude.x), initAmplitude.y + Random.Range(-initAmplitude.y, initAmplitude.y));
    //    Vector2 newFrequency = new Vector2(initFrequency.x + Random.Range(-initFrequency.x, initFrequency.x), initFrequency.y + Random.Range(-initFrequency.y, initFrequency.y));

    //    amplitude = newAmplitude;
    //    frequency = newFrequency;

    //    StartCoroutine(RandomizeOscillatingEffect());
    //}
}
