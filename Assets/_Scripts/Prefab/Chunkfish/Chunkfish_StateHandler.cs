using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chunkfish_StateHandler : MonoBehaviour
{
    [SerializeField] private AudioClip sfx_chunkfishFullyInflated;
    [SerializeField] private float chunkfish_sfxMaxRange = 30f;

    [Header("Current Detected Object")]
    public GameObject detectedObject;

    [Header("Colliders")]
    [SerializeField] private Collider2D _chunkfishDetectionCollider;

    [Header("States")]
    public bool isDetecting;
    public bool isInflated;
    public bool isFullyInflated;
    private bool isStayingFullyInflated; // just for controlling sfx, to trigger this bool once

    [Header("Inflation Variables")]
    [SerializeField] public float chunkfish_inflateTimer;
    [SerializeField] private float chunkfish_deflateSpeed;
    [SerializeField] private float chunkfish_fullyInflatedSpeed;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _isChunkfishDetectable;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_isChunkfishDetectable.name) == true)
            {
                detectedObject = collision.gameObject;
                Inflate();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_isChunkfishDetectable.name) == true && CheckExistingObjects() == false)
            {
                Deflate();
            }

        }
    }

    private void Inflate()
    {
        isInflated = true;
        isDetecting = true;
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_chunkfishDetectionCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(_isChunkfishDetectable.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Deflate()
    {
        isDetecting = false;
        chunkfish_inflateTimer = chunkfish_deflateSpeed;
    }

    private void FixedUpdate()
    {
        if (chunkfish_inflateTimer >= chunkfish_fullyInflatedSpeed)
        {
            isFullyInflated = true;
            if (isStayingFullyInflated == false)
            {
                PlayFullyInflatedSFX();
            }

        } else if (chunkfish_inflateTimer <= 0)
        {
            isFullyInflated = false;
        }

        if (!isDetecting) {
            chunkfish_inflateTimer = Mathf.Clamp(chunkfish_inflateTimer - Time.deltaTime, 0, 9999);

            if (chunkfish_inflateTimer <= 0)
            {
                isInflated = false;
                isStayingFullyInflated = false;
            }
        } else
        {
            chunkfish_inflateTimer = Mathf.Clamp(chunkfish_inflateTimer + Time.deltaTime, 0, 9999);
        }
    }

    private void PlayFullyInflatedSFX()
    {
        isStayingFullyInflated = true;
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_chunkfishFullyInflated, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f, 1f, 1f, chunkfish_sfxMaxRange);
    }
}
