using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakableWarehouseBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TagsScriptObj tag_player;
    [SerializeField] private GameObject pivotPoint;

    [Header("Settings")]
    [SerializeField] private float velocityBreakingPoint = 11f;
    [SerializeField] private float xVelocityWeight = 1f;
    [SerializeField] private float yVelocityWeight = 0.75f;

    [Header("Box References")]
    [SerializeField] private Vector2 currentPlayerVelocity;
    [SerializeField] private GameObject[] brokenBoxPieces;
    [SerializeField] private AudioClip[] brokenBoxSounds;

    [SerializeField] private int minWarehouseJunk = 1;
    [SerializeField] private int maxWarehouseJunk = 3;

    [Serializable]
    public class JunkData
    {
        public GameObject junkObject;
        public int weight;
    }

    [SerializeField] private JunkData[] junkData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                CheckPlayerVelocity(collision);
            }
        }
    }

    private void CheckPlayerVelocity(Collider2D collision)
    {
        // Only breaks box if going fast enough
        if (collision.TryGetComponent<Rigidbody2D>(out var rb))
        {
            float combinedVelocity = Mathf.Abs(rb.velocity.x * xVelocityWeight) + Mathf.Abs(rb.velocity.y * yVelocityWeight);
            if (combinedVelocity > velocityBreakingPoint)
            {
                currentPlayerVelocity = rb.velocity;
                SpawnBrokenBoxPieces(currentPlayerVelocity);
                GenerateWarehouseJunk();

                Manager_SFXPlayer.instance.PlayRandomSFXClip(brokenBoxSounds, transform, 0.1f, false, Manager_AudioMixer.instance.mixer_sfx, true, 0.3f);
                Destroy(gameObject);
            }
        }
    }

    private void SpawnBrokenBoxPieces(Vector2 currentVelocity)
    {
        foreach (GameObject box in brokenBoxPieces)
        {
            GameObject brokenBoxPiece = Instantiate(box, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            Vector2 modifiedVelocity = new Vector2(currentVelocity.x, Mathf.Clamp(currentVelocity.y, 10f, 999f));

            modifiedVelocity = new Vector2(modifiedVelocity.x + Random.Range(-15, 15), modifiedVelocity.y + Random.Range(-15, 15));

            brokenBoxPiece.GetComponent<Rigidbody2D>().velocity = modifiedVelocity;

            brokenBoxPiece.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-150f, 150f);
        }
    }

    private void GenerateWarehouseJunk()
    {
        int junkAmount = Random.Range(minWarehouseJunk, maxWarehouseJunk + 1);
        for (int i = 0; i < junkAmount; i++)
        {
            GenerateWeightPoolWarehouseJunk();
        }
    }

    private void GenerateWeightPoolWarehouseJunk()
    {
        int junkWeightTotal = 0;
        foreach (JunkData junk in junkData)
        {
            junkWeightTotal += junk.weight;
        }

        int random = Random.Range(0, junkWeightTotal);

        foreach (JunkData junk in junkData)
        {
            if (random < junk.weight)
            {
                SpawnWarehouseJunk(junk.junkObject);
                return;
            }
            random -= junk.weight;
        }
    }

    private int RandomSign()
    {
        return Random.value < .5? 1 : -1;
    }

    private void SpawnWarehouseJunk(GameObject junk)
    {
        GameObject warehouseJunk = Instantiate(junk, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

        Vector2 modifiedVelocity = new Vector2(Random.Range(5, 10) * RandomSign(), Random.Range(5, 10));

        warehouseJunk.GetComponent<Rigidbody2D>().velocity = modifiedVelocity;

        warehouseJunk.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(100f, 200f) * RandomSign();

    }
}