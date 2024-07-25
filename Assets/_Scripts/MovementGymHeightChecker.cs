using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MovementGymHeightChecker : MonoBehaviour
{
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private float highestHeight;
    [SerializeField] private GameObject heightMarker;
    [SerializeField] private GameObject currentMarker;
    [SerializeField] private bool hasFallen;

    private void Awake()
    {
        highestHeight = Mathf.NegativeInfinity;
        hasFallen = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (rb.velocity.y <= 0f)
            {
                hasFallen = true;
            }

            if (rb.velocity.y > 0.1f && hasFallen)
            {
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
                currentMarker.transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
            }
        }
    }
}
