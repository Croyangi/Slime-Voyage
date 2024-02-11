using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunkfish_Bounce : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject chunkfish;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    [Header("Current Collided Object")]
    [SerializeField] private GameObject collidedObject;
    [SerializeField] private Rigidbody2D collidedObject_rb;

    [Header("Variables")]
    [SerializeField] private float chunkfish_bounceStrength;
    [SerializeField] private float chunkfish_additionalVerticleBounceStrength;
    [SerializeField] private float chunkfish_movementStallTime;
    [SerializeField] private float chunkfish_deccelerationStallTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidedObject = collision.gameObject;
        if (collidedObject.GetComponent<Rigidbody2D>() != null)
        {
            collidedObject_rb = collision.GetComponent<Rigidbody2D>();
            OnChunkfishBounce();
        }


    }

    private float GetAngle()
    {
        float x1 = chunkfish.transform.position.x;
        float x2 = collidedObject.transform.position.x;
        float y1 = chunkfish.transform.position.y;
        float y2 = collidedObject.transform.position.y;

        float angle = Mathf.Atan2(y2 - y1, x2 - x1) * (180 / Mathf.PI);
        angle = (angle + 360) % 360;
        return angle;
    }

    private void OnChunkfishBounce()
    {
        // Find angle to apply velocity
        float angle = GetAngle();
        float angleRadians = angle * Mathf.Deg2Rad;
        float velocityX = Mathf.Cos(angleRadians);
        float velocityY = Mathf.Sin(angleRadians);

        // Helps players get over chunkfishes
        velocityY += chunkfish_additionalVerticleBounceStrength;

        // Apply velocity
        collidedObject_rb.velocity = Vector2.zero;
        Vector2 blastVelocity = new Vector2(velocityX, velocityY) * chunkfish_bounceStrength;
        collidedObject_rb.AddForce(blastVelocity, ForceMode2D.Impulse);

        // Set extra stalls
        if (collidedObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true && collidedObject.GetComponent<IMovementProcessor>() != null)
            {
                SetMovementStall();
            }
        }
    }

    private void SetMovementStall()
    {
        IMovementProcessor movementStallComponent = collidedObject.GetComponent<IMovementProcessor>();

        float increase = chunkfish_bounceStrength - 35f; // Original strength is 35, thus subtracting 35 from it
        float percentChange = (increase / chunkfish_bounceStrength) + 1;

        if (percentChange != 1)
        {
            movementStallComponent.SetMovementStall(chunkfish_movementStallTime * (percentChange * 1.5f));
        }
        else
        {
            movementStallComponent.SetMovementStall(chunkfish_movementStallTime);
        }
    }
}
