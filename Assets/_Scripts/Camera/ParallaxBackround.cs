using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParallaxBackround : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector2 length;
    [SerializeField] private Vector2 startPos;
    [SerializeField] GameObject cam;

    [Header("Variables")]
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float distance;

    // Start is called before the first frame update
    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    private void Update()
    {
        float tempX = (cam.transform.transform.position.x * (1 - parallaxEffect));
        float tempY = (cam.transform.transform.position.y * (1 - parallaxEffect));

        float distanceX = (cam.transform.position.x * parallaxEffect);
        float distanceY = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);


        // X-axis version
        if (tempX > startPos.x + length.x)
        {
            startPos.x += length.x;
        } else if (tempX < startPos.x - length.x)
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
