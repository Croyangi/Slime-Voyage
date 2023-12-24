using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlimeDeath : MonoBehaviour
{
    [SerializeField] private float animationTime;
    [SerializeField] private SpriteRenderer sr;

    private void FixedUpdate()
    {
        animationTime -= Time.deltaTime;

        if (animationTime < 0)
        {
            sr.enabled = false;
        }

        if (animationTime < -10)
        {
            Destroy(gameObject);
        }
    }
}
