using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowBulb : MonoBehaviour
{
    [SerializeField] private GameObject glowBulb;
    [SerializeField] private Vector2 initPos;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private Vector2 amplitude;
    [SerializeField] private Vector2 frequency;

    [SerializeField] private Vector2 initAmplitude;

    [SerializeField] private float elapsedTime;

    private void Awake()
    {
        initPos = glowBulb.gameObject.transform.position;

        // Instantiate initial variables to base randomness off
        amplitude = initAmplitude;

        StartCoroutine(RandomizeOscillatingEffect());
    }

    private void OnEnable()
    {
        glowBulb.gameObject.transform.position = initPos;
        StartCoroutine(RandomizeOscillatingEffect());
    }

    private void OnDisable()
    {
        glowBulb.transform.position = initPos;
    }

    private void FixedUpdate()
    {
        // Oscillates in a circular motion
        elapsedTime += Time.deltaTime;

        float glowBulbX = Mathf.Cos(elapsedTime * frequency.x) * amplitude.x;
        float glowBulbY = Mathf.Sin(elapsedTime * frequency.y) * amplitude.y;

        glowBulb.transform.position = new Vector2(glowBulb.transform.position.x + glowBulbX, glowBulb.transform.position.y + glowBulbY);
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