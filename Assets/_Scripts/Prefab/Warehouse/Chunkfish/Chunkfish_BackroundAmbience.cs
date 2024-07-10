using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunkfish_BackroundAmbience : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cam;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Sprite deflatedChunkfish;
    [SerializeField] private SpriteRenderer sr;

    [Header("Variables")]
    [SerializeField] private float risingAmount;
    [SerializeField] private float deathTimer;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float distanceX;
    [SerializeField] private float offset;

    [Header("Rise/Fall Settings")]
    [SerializeField] private float _amplitudeRotate = 0;
    [SerializeField] private float _frequencyRotate = 1;

    private void Start()
    {
        offset = Random.Range(-15f, 15f);
        startPos = transform.position;
        startPos.x += offset;

        cam = GameObject.FindWithTag("MainCamera");
    }

    private void FixedUpdate()
    {
        float tempX = (cam.transform.transform.position.x * (1 - parallaxEffect));

        distanceX = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos.x + distanceX, transform.position.y + risingAmount, transform.position.z);

        // X-axis version
        if (tempX > startPos.x + 25)
        {
            startPos.x += 50;
        }
        else if (tempX < startPos.x - 25)
        {
            startPos.x -= 50;
        }


        if (deathTimer < 10f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            transform.Rotate(new Vector3(0, 0, 3f));
            sr.sprite = deflatedChunkfish;

        } else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + risingAmount, transform.position.z);
            float rotateZ = Mathf.Sin(Time.time * _frequencyRotate) * _amplitudeRotate;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
        }

        if (deathTimer < 0f)
        {
            Destroy(gameObject);
        }

        deathTimer -= Time.deltaTime;
    }
}
