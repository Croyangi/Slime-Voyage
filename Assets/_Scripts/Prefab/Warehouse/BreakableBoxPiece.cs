using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBoxPiece : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float alpha;
    [SerializeField] private float fadeOutTime = 1;
    [SerializeField] private float time = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        alpha = Mathf.Lerp(1f, 0f, time / fadeOutTime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
