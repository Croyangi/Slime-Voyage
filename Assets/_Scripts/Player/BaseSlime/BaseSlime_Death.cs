using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_Death : MonoBehaviour, IPlayerProcessor
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] slimeParticles;
    [SerializeField] private int particleAmount;

    [Header("Settings")]
    [SerializeField] private float xVelocityPointWeight = 1f;
    [SerializeField] private float yVelocityPointWeight = 1f;
    [SerializeField] private float velocityPoint = 1f;

    [SerializeField] private float xVelocityParticleWeight = 1f;
    [SerializeField] private float yVelocityParticleWeight = 1f;

    [SerializeField] private Vector2 minVelocityRandomParticle;
    [SerializeField] private Vector2 maxVelocityRandomParticle;

    [SerializeField] private Vector2 minVelocityIdleRandomParticle;
    [SerializeField] private Vector2 maxVelocityIdleRandomParticle;

    [SerializeField] private AudioClip sfx_slimeDeath;

    public void InitiatePlayerDeath()
    {
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_slimeDeath, transform, 0.1f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.1f);

        for (int i = 0; i < particleAmount; i++)
        {

            // Spawn particles based on player velocity, but if not moving, spread randomly
            // But also 50/50 check because a shotgun spray in one direction doesn't look great

            float combinedVelocity = Mathf.Abs(rb.velocity.x * xVelocityPointWeight) + Mathf.Abs(rb.velocity.y * yVelocityPointWeight);
            if (combinedVelocity < velocityPoint)
            {
                SpawnRandomSlimeParticle();

            } else
            {
                if (RandomSign() < 0)
                {
                    SpawnSlimeParticle();
                } else
                {
                    SpawnRandomSlimeParticle();
                }
            }
        }

        Destroy(rb.gameObject);
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
        Vector2 randomVelocity = new Vector2(Random.Range(minVelocityRandomParticle.x, maxVelocityRandomParticle.x) * RandomSign(), Random.Range(minVelocityRandomParticle.y, maxVelocityRandomParticle.y) * RandomSign());

        modifiedVelocity += randomVelocity;
        slimeParticle.GetComponent<Rigidbody2D>().velocity = modifiedVelocity;
    }

    private void SpawnRandomSlimeParticle()
    {
        int randomIndex = Random.Range(0, slimeParticles.Length);

        // Apply some randomness to where it spawns
        Vector2 randomPos = new Vector2(Random.Range(0.1f, 0.5f) * RandomSign(), Random.Range(0.1f, 0.5f) * RandomSign());
        GameObject slimeParticle = Instantiate(slimeParticles[randomIndex], new Vector2(transform.position.x + randomPos.x, transform.position.y + randomPos.y), Quaternion.identity);

        // Apply some randomness to it
        Vector2 randomVelocity = new Vector2(Random.Range(minVelocityIdleRandomParticle.x, maxVelocityIdleRandomParticle.x) * RandomSign(), Random.Range(minVelocityIdleRandomParticle.y, maxVelocityIdleRandomParticle.y) * RandomSign());

        slimeParticle.GetComponent<Rigidbody2D>().velocity = randomVelocity;
    }

    private int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }
}

