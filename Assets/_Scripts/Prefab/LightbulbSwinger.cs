using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightbulbSwinger : GeneralCullerCommunicator
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private float swingStrengthMultiplier;
    [SerializeField] private float swingStrengthMax;
    [SerializeField] private float xVelocityWeight = 1f;
    [SerializeField] private float yVelocityWeight = 1f;

    [SerializeField] private float swingCooldownTimer;
    [SerializeField] private float swingCooldown;

    [SerializeField] private Rigidbody2D rb_lightbulb;
    [SerializeField] private BoxCollider2D col_lightbulb;
    [SerializeField] private GameObject lightbulb;
    [SerializeField] private SpriteRenderer sr_lightbulb;
    [SerializeField] private SpriteRenderer sr_wire;
    [SerializeField] private HingeJoint2D hinge_lightbulb;

    [SerializeField] private Sprite lightbulbOn;
    [SerializeField] private Sprite lightbulbOff;
    [SerializeField] private Light2D[] lights;
    [SerializeField] private float length;

    private void Awake()
    {
        // Set length
        length = sr_wire.size.y;
        ChangeLightbulbLength(length);

        // Set swing strength offset based on wire length
        float offset = 5f - length;
        if (offset > 0)
        {
            swingStrengthMultiplier -= offset * 0.3f;
            swingStrengthMultiplier = Mathf.Clamp(swingStrengthMultiplier, 0.05f, 999);
        }
    }

    public override void OnLoad()
    {
        rb_lightbulb.simulated = true;

        if (GetRandomChance(0.5f))
        {
            float appliedForce = Random.Range(1f, 3f);
            if (GetRandomChance(0.5f))
            {
                appliedForce *= -1;
            }
            ApplyVelocity(appliedForce);
        }
    }

    public override void OnCull()
    {
        rb_lightbulb.angularVelocity = 0f;
        rb_lightbulb.velocity = Vector2.zero;
        sr_wire.transform.rotation = Quaternion.identity;

        rb_lightbulb.simulated = false;
    }

    public override void FixedUpdateState()
    {
        if (Mathf.Abs(transform.parent.rotation.z) > 40)
        {
            rb_lightbulb.angularVelocity = 0f;
        }
    }

    public void ChangeLightbulbLength(float newLength)
    {
        length = newLength;
        sr_wire.size = new Vector2(sr_wire.size.x, length);
        col_lightbulb.size = new Vector2(col_lightbulb.size.x, length);
        hinge_lightbulb.anchor = new Vector2(hinge_lightbulb.anchor.x, length / 2f);
        lightbulb.transform.localPosition = new Vector2(lightbulb.transform.localPosition.x, -(length / 2f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // omitted player tag check
        if (swingCooldownTimer <= 0f)
        {
            CheckPlayerVelocity(collision);

            if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_player.name) == true)
                {
                    if (GetRandomChance(0.2f)) { StartCoroutine(LightbulbFlicker()); }
                }
            }
        }

        //if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        //{
        //    // omitted player tag check
        //    if (swingCooldownTimer <= 0f)
        //    {
        //        CheckPlayerVelocity(collision);
        //    }
        //}
    }

    private IEnumerator LightbulbFlicker()
    {
        ToggleLights();
        yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
        ToggleLights();
        yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));

        for (int i = 0; i < Random.Range(1, 2f); i++)
        {
            ToggleLights();
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            ToggleLights();
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
    }

    public void ToggleLights()
    {
        foreach (Light2D light in lights)
        {
            light.enabled = !light.enabled;
        }

        if (lights[0].enabled)
        {
            sr_lightbulb.sprite = lightbulbOn;
        }
        else
        {
            sr_lightbulb.sprite = lightbulbOff;
        }
    }

    private bool GetRandomChance(float chance)
    {
        return (Random.value < chance);
    }

    private void CheckPlayerVelocity(Collider2D collision)
    {
        // Checks velocity and applies it to the lightbulb
        if (collision.TryGetComponent<Rigidbody2D>(out var rb))
        {
            float appliedForce = (rb.velocity.x * xVelocityWeight) + (rb.velocity.y * yVelocityWeight);
            ApplyVelocity(appliedForce);
        }
    }

    private void ApplyVelocity(float force)
    {
        force *= swingStrengthMultiplier;
        force = Mathf.Clamp(force, -swingStrengthMax, swingStrengthMax);

        rb_lightbulb.AddTorque(force, ForceMode2D.Impulse);

        swingCooldownTimer = swingCooldown;
        StartCoroutine(CooldownTimer());
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
