using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementGymHeightChecker : MonoBehaviour
{
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private float highestHeight;
    [SerializeField] private GameObject heightMarker;
    [SerializeField] private GameObject currentMarker;
    [SerializeField] private bool hasFallen;
    [SerializeField] private float yPositionOffset;

    [SerializeField] private float highestHeightTimer;
    [SerializeField] private TMP_Text tmp_timer;

    private void Awake()
    {
        highestHeight = Mathf.NegativeInfinity;
        hasFallen = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (rb.velocity.y > 0.01f)
            {
                highestHeightTimer += Time.deltaTime;
                ChangeText(highestHeightTimer);
            }

            if (rb.velocity.y <= 0f)
            {
                hasFallen = true;
            }

            if (rb.velocity.y > 0.01f && hasFallen)
            {
                highestHeightTimer = 0f;
                ChangeText(highestHeightTimer);

                hasFallen = false;
                highestHeight = Mathf.NegativeInfinity;
                currentMarker = Instantiate(heightMarker, collision.transform.position, Quaternion.identity);
            }
        }

        if (collision.transform.position.y != currentPosition.y)
        {
            OnChangedHeight(collision.transform);
        }
        currentPosition = collision.transform.position;
    }

    private void OnChangedHeight(Transform transform)
    {
        // Check if new height record
        if (currentPosition.y > highestHeight)
        {
            highestHeight = currentPosition.y;
            if (currentMarker != null)
            {
                tmp_timer.gameObject.transform.position = new Vector2(transform.position.x - 6f, transform.position.y);
                currentMarker.transform.position = new Vector2(transform.position.x, transform.position.y + yPositionOffset);
            }
        }
    }

    private void ChangeText(float time)
    {
        TimeSpan textTime = TimeSpan.FromSeconds(time);
        string text = textTime.Seconds.ToString("00") + "." +
                              textTime.Milliseconds.ToString("000");
        tmp_timer.text = text;
    }
}
