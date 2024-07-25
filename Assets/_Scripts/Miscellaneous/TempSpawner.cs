using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col_spawner;
    [SerializeField] private float spawnerCooldown;
    [SerializeField] private bool isSpawning;
    [SerializeField] private GameObject[] spawnedObjs;

    private void Awake()
    {
        InitiateSpawner();
    }

    [ContextMenu("Initiate Spawner")]
    public void InitiateSpawner()
    {
        isSpawning = true;
        StartCoroutine(SpawnerTimer());
    }

    private IEnumerator SpawnerTimer()
    {
        Spawn();
        yield return new WaitForSeconds(spawnerCooldown);
        if (isSpawning)
        {
            StartCoroutine(SpawnerTimer());
        }
    }

    private void Spawn()
    {
        int random = Random.Range(0, spawnedObjs.Length);

        float xRange = Random.Range(-col_spawner.bounds.extents.x, col_spawner.bounds.extents.x) + col_spawner.bounds.center.x;
        float yRange = Random.Range(-col_spawner.bounds.extents.y, col_spawner.bounds.extents.y) + col_spawner.bounds.center.y;

        Vector2 pos = new Vector2(xRange, yRange);

        GameObject spawned = Instantiate(spawnedObjs[random], pos, Quaternion.identity);
        spawned.GetComponent<Rigidbody2D>().drag += Random.Range(-0.1f, 0.1f);
        spawned.GetComponent<Rigidbody2D>().gravityScale += Random.Range(-0.1f, 0.1f);
    }
}
