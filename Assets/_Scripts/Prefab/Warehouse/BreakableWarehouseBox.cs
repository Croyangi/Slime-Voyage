using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWarehouseBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private Transform originPoint;
    [SerializeField] private float initialRotation;

    [Header("Settings")]
    [SerializeField] private float velocityBreakingPoint = 11f;
    [SerializeField] private float xVelocityWeight = 1f;
    [SerializeField] private float yVelocityWeight = 0.75f;

    [Header("Box Pieces")]
    [SerializeField] private GameObject[] brokenBoxPieces;

    private void Awake()
    {
        originPoint = transform;
        initialRotation = gameObject.transform.eulerAngles.z;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                MoveBox(collision.transform);

                // Only breaks box if going fast enough
                if (collision.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    float combinedVelocity = Mathf.Abs(rb.velocity.x * xVelocityWeight) + Mathf.Abs(rb.velocity.y * yVelocityWeight);
                    if (combinedVelocity > velocityBreakingPoint)
                    {
                        SpawnBrokenBoxPieces(rb.velocity);
                        //SpawnBrokenBoxPieces(rb.velocity, collision.ClosestPoint(collision.transform.position));
                    }
                }
            }
        }
    }

    private void MoveBox(Transform playerPos)
    {

        if (originPoint.position.x < playerPos.position.x) // Player on right
        {
            LeanTween.rotateZ(gameObject, initialRotation + 5, 0.1f);
            LeanTween.rotateZ(gameObject, initialRotation - 5, 0.1f).setDelay(0.1f);
        }
        else if (originPoint.position.x > playerPos.position.x) // Player on left
        {
            LeanTween.rotateZ(gameObject, initialRotation - 5, 0.1f);
            LeanTween.rotateZ(gameObject, initialRotation + 5, 0.1f).setDelay(0.1f);
        }

        LeanTween.rotateZ(gameObject, initialRotation, 0).setDelay(0.3f);
    }

    private void SpawnBrokenBoxPieces(Vector2 currentVelocity)
    {
        foreach (GameObject box in brokenBoxPieces)
        {
            GameObject brokenBoxPiece = Instantiate(box, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            Vector2 modifiedVelocity = new Vector2(currentVelocity.x, Mathf.Clamp(currentVelocity.y, 10f, 999f));
            brokenBoxPiece.GetComponent<Rigidbody2D>().velocity = modifiedVelocity;

            brokenBoxPiece.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-150f, 150f);
        }

        Destroy(gameObject);
    }
}
