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
    [SerializeField] private float bounds_minX;
    [SerializeField] private float bounds_maxX;
    [SerializeField] private float bounds_minY;
    [SerializeField] private float bounds_maxY;

    [Header("Variables")]
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float distance;

    // Start is called before the first frame update
    private void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    private void Update()
    {

        float tempX = (_camera.transform.transform.position.x * (1 - parallaxEffect));
        float distanceX = (_camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos.x + distanceX, _camera.transform.position.y, transform.position.z);


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
