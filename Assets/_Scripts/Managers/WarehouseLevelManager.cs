using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseLevelManager : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject chunkfish;
    [SerializeField] private float chunkfishTimer;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
    }

    private bool GetRandomBool()
    {
        return (Random.value > 0.5f);
    }

    private void FixedUpdate()
    {
        chunkfishTimer -= Time.deltaTime;

        if (chunkfishTimer < 0f)
        {
            SpawnChunkfish();
            chunkfishTimer = Random.Range(15f, 20f);
        }
    }

    private void SpawnChunkfish()
    {
        Vector3 chunkfishRandomX = new Vector3(cam.transform.position.x, cam.transform.position.y - 15f, 50f);
        Instantiate(chunkfish, chunkfishRandomX, Quaternion.identity);

    }
}
