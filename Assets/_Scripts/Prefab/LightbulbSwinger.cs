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
    [SerializeField] private Light2D light2d;
    [SerializeField] private bool isInversed;

    private void Start()
    {
        if (isInversed)
        {
            light2d.enabled = false;
            sr_lightbulb.sprite = lightbulbOff;
        }
    }

    public override void OnLoad()
    {
        float appliedForce = Random.Range(3f, 5f);
        if (GetRandomChance(0.5f))
        {
            appliedForce *= -1;
        }

        rb_lightbulb.AddTorque(appliedForce, ForceMode2D.Impulse);
        Debug.Log("Applying: " + appliedForce);

        swingCooldownTimer = swingCooldown;
        StartCoroutine(CooldownTimer());
    }

    public void ChangeLightbulbLength(float length)
    {
        sr_wire.size = new Vector2(sr_wire.size.x, length);
        col_lightbulb.size = new Vector2(col_lightbulb.size.x, length);
        hinge_lightbulb.anchor = new Vector2(hinge_lightbulb.anchor.x, length / 2f);
        Debug.Log(-(length / 2f) - 0.5f);
        lightbulb.transform.localPosition = new Vector2(lightbulb.transform.localPosition.x, -(length / 2f) - 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // omitted player tag check
        if (swingCooldownTimer <= 0f)
        {
            CheckPlayerVelocity(collision);
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

    private void ToggleLights()
    {
        light2d.enabled = !light2d.enabled;
        if (light2d.enabled)
        {
            sr_lightbulb.sprite = lightbulbOn;
        } else
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
            if (GetRandomChance(0.2f)) { StartCoroutine(LightbulbFlicker()); }

            float appliedForce = (rb.velocity.x * xVelocityWeight) + (rb.velocity.y * yVelocityWeight);
            appliedForce *= swingStrengthMultiplier;
            appliedForce = Mathf.Clamp(appliedForce, -swingStrengthMax, swingStrengthMax);

            ApplyVelocity(appliedForce);
        }
    }

    private void ApplyVelocity(float force)
    {
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
