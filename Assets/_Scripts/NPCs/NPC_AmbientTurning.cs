using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AmbientTurning : MonoBehaviour
{
    [Header("Self-References")]
    [SerializeField] private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(AmbientTurningRandomChance());
    }

    private IEnumerator AmbientTurningRandomChance()
    {
        sr.flipX = !(sr.flipX);

        float random = Random.Range(1f, 5f);
        yield return new WaitForSeconds(random);
        StartCoroutine(AmbientTurningRandomChance());
    }
}
