using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteFadeOut : MonoBehaviour
{
    [SerializeField] private float startingAlpha;
    [SerializeField] private float target;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private bool isDestroyedWhenReachingTarget;

    private void Awake()
    {
        Color spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, startingAlpha);
    }

    private void FixedUpdate()
    {
        Color spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        float newAlpha = Mathf.MoveTowards(spriteColor.a, target, transitionSpeed * Time.deltaTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, newAlpha);

        if (newAlpha == target)
        {
            if (isDestroyedWhenReachingTarget)
            {
                Destroy(gameObject);
            }
        }
    }
}
