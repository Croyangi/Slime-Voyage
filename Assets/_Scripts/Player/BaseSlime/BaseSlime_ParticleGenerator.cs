using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_ParticleGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] slimeParticles;

    [Header("Settings")]
    [SerializeField] private float slimeOffset = 0.5f; // Time between each particle
    [SerializeField] private float slimeOffsetTime = 0f; // Current slime offset time

    [SerializeField] private float xVelocityPointWeight = 1f;
    [SerializeField] private float yVelocityPointWeight = 0.75f;
    [SerializeField] private float velocityPoint = 5f;

    [SerializeField] private float xVelocityParticleWeight = 1f;
    [SerializeField] private float yVelocityParticleWeight = 1f;

    private void FixedUpdate()
    {
        if (slimeOffsetTime > 0f)
        {
            slimeOffsetTime -= Time.deltaTime;
        }

        float combinedVelocity = Mathf.Abs(rb.velocity.x * xVelocityPointWeight) + Mathf.Abs(rb.velocity.y * yVelocityPointWeight);
        if (combinedVelocity > velocityPoint && slimeOffsetTime <= 0f)
        {
            slimeOffsetTime = slimeOffset;

            SpawnSlimeParticle();

        }
    }

    private void SpawnSlimeParticle()
    {
        int randomIndex = Random.Range(0, slimeParticles.Length);

        // Apply some randomness to where it spawns
        Vector2 randomPos = new Vector2(Random.Range(0.1f, 0.5f) * RandomSign(), Random.Range(0.1f, 0.5f) * RandomSign());
        GameObject slimeParticle = Instantiate(slimeParticles[randomIndex], new Vector2(transform.position.x + randomPos.x, transform.position.y + randomPos.y), Quaternion.identity);

        // Apply velocity in opposite vector as slime
        Vector2 modifiedVelocity = new Vector2(-rb.velocity.x * xVelocityParticleWeight, -rb.velocity.y * yVelocityParticleWeight);

        // Apply some randomness to it
        Vector2 randomVelocity = new Vector2(Random.Range(1, 3) * RandomSign(), Random.Range(1, 3) * RandomSign());

        modifiedVelocity += randomVelocity;
        slimeParticle.GetComponent<Rigidbody2D>().velocity = modifiedVelocity;
    }

    private int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }

}
 