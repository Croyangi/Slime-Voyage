using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OminousEmployerMovement : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject[] eyes;
    [SerializeField] private GameObject paper;
    [SerializeField] private Rigidbody lampRb;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float eyes_amplitude = 0;
    [SerializeField] private float eyes_frequency = 1;
    [SerializeField] private float paper_amplitude = 0;
    [SerializeField] private float paper_frequency = 1;
    [SerializeField] private float paper_timeOffset;
    [SerializeField] private float time;

    private void OnEnable()
    {
        paper_timeOffset = Random.Range(1, 5);
        //lampRb.AddTorque(new Vector3(0.2f, 0, 0.4f), ForceMode.Impulse);
    }

    public void FixedUpdate()
    {
        time += Time.deltaTime;
        EyesHoverEffect(time);
        PaperHoverEffect(time + paper_timeOffset);
    }

    private void EyesHoverEffect(float eyeTime)
    {
        float y = Mathf.Sin(eyeTime * eyes_frequency) * eyes_amplitude;
        foreach (GameObject eye in eyes)
        {
            eye.transform.position = new Vector3(eye.transform.position.x, eye.transform.position.y + y, eye.transform.position.z);
        }
    }

    private void PaperHoverEffect(float paperTime)
    {
        float y = Mathf.Sin(paperTime * paper_frequency) * paper_amplitude;
        paper.transform.position = new Vector3(paper.transform.position.x, paper.transform.position.y + y, paper.transform.position.z);
    }
}
