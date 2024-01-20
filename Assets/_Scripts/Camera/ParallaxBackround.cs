using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParallaxBackround : MonoBehaviour
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

    private Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float cameraHeight = camera.GetComponent<Camera>().orthographicSize * 2;
        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    // Update is called once per frame
    private void Update()
    {
        //bounds = OrthographicBounds(_camera);
        //bounds_minX = bounds.min.x;
        //bounds_maxX = bounds.max.x;
        //bounds_minY = bounds.min.y;
        //bounds_maxY = bounds.max.y;

        float tempX = (_camera.transform.transform.position.x * (1 - parallaxEffect));
        float tempY = (_camera.transform.transform.position.y * (1 - parallaxEffect));

        float distanceX = (_camera.transform.position.x * parallaxEffect);
        float distanceY = (_camera.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);


        // X-axis version
        if (tempX > startPos.x + length.x)
        {
            startPos.x += length.x;
        }
        else if (tempX < startPos.x - length.x)
        {
            startPos.x -= length.x;
        }

        // Y-axis version
        if (tempY > startPos.y + length.y)
        {
            startPos.y += length.y;
        }
        else if (tempY < startPos.y - length.y)
        {
            startPos.y -= length.y;
        }
    }
}
