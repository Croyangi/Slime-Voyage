using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoosterParticle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float timer = 1f;

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Destroy(gameObject);
        }
    }
}
