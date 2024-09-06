using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbSwinger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private float swingStrengthMultiplier;
    [SerializeField] private float swingStrengthMax;
    [SerializeField] private float xVelocityWeight = 1f;
    [SerializeField] private float yVelocityWeight = 1f;

    [SerializeField] private float swingCooldownTimer;
    [SerializeField] private float swingCooldown;

    [SerializeField] private Rigidbody2D lightbulbRb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && swingCooldownTimer <= 0f)
            {
                CheckPlayerVelocity(collision);
            }
        }
    }

    private void CheckPlayerVelocity(Collider2D collision)
    {
        // Checks velocity and applies it to the lightbulb
        if (collision.TryGetComponent<Rigidbody2D>(out var rb))
        {
            float appliedForce = (rb.velocity.x * xVelocityWeight) + (rb.velocity.y * yVelocityWeight);
            appliedForce *= swingStrengthMultiplier;
            appliedForce = Mathf.Clamp(appliedForce, -swingStrengthMax, swingStrengthMax);

            lightbulbRb.AddTorque(appliedForce, ForceMode2D.Impulse);

            swingCooldownTimer = swingCooldown;
            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator CooldownTimer()
    {
        swingCooldownTimer -= Time.deltaTime;
        yield return new WaitForFixedUpdate();
        if (swingCooldownTimer > 0f)
        {
            StartCoroutine(CooldownTimer());
        }
    }
}
