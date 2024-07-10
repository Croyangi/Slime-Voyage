using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientFly : MonoBehaviour
{
    [SerializeField] private GameObject fly;
    [SerializeField] private Vector2 initPos;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private Vector2 amplitude;
    [SerializeField] private Vector2 frequency;

    [SerializeField] private Vector2 initAmplitude;

    [SerializeField] private float elapsedTime;

    private void Awake()
    {
        initPos = fly.gameObject.transform.position;

        // Instantiate initial variables to base randomness off
        amplitude = initAmplitude;

        StartCoroutine(RandomizeOscillatingEffect());
    }

    private void OnEnable()
    {
        fly.gameObject.transform.position = initPos;
        StartCoroutine(RandomizeOscillatingEffect());
    }

    private void OnDisable()
    {
        fly.gameObject.transform.position = initPos;
    }

    private void FixedUpdate()
    {
        // Oscillates in a circular motion
        elapsedTime += Time.deltaTime;

        float flyX = Mathf.Cos(elapsedTime * frequency.x) * amplitude.x;
        float flyY = Mathf.Sin(elapsedTime * frequency.y) * amplitude.y;

        fly.transform.position = new Vector2(fly.transform.position.x + flyX, fly.transform.position.y + flyY);
    }

    // Randomizes the circular effect, to be less... circular
    private IEnumerator RandomizeOscillatingEffect()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        Vector2 newAmplitude = new Vector2(Random.Range(-initAmplitude.x, initAmplitude.x), Random.Range(-initAmplitude.y, initAmplitude.y));
        //Vector2 newFrequency = new Vector2(initFrequency.x + Random.Range(-initFrequency.x, initFrequency.x), initFrequency.y + Random.Range(-initFrequency.y, initFrequency.y));

        amplitude = newAmplitude;

        StartCoroutine(RandomizeOscillatingEffect());
    }
}
