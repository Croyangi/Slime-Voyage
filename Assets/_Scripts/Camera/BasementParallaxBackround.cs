using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementParallaxBackround : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector2 length;
    [SerializeField] private Vector2 startPos;
    [SerializeField] Camera _camera;
    [SerializeField] private Bounds bounds;

    [SerializeField] private float offsetY;

    [Header("Variables")]
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float distance;

    private void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    private void Update()
    {
        float tempX = (_camera.transform.transform.position.x * (1 - parallaxEffect));
        float distanceX = (_camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos.x + distanceX, _camera.transform.position.y + offsetY, transform.position.z);


        // X-axis version
        if (tempX > startPos.x + length.x)
        {
            startPos.x += length.x;
        }
        else if (tempX < startPos.x - length.x)
        {
            startPos.x -= length.x;
        }
    }
}
