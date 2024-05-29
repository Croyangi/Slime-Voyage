using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private bool isCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected)
        {
            StartCoroutine(OnCollected());
            isCollected = true;
        }
    }

    private IEnumerator OnCollected()
    {
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.1f);
        LeanTween.scale(gameObject, new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setDelay(0.1f);
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 1f).setDelay(0.2f).setEaseOutBounce();
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }
}
