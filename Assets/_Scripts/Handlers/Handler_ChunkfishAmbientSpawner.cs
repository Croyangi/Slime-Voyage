using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Handler_ChunkfishAmbientSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject backroundChunkfish;

    private void Awake()
    {
        StartCoroutine(SpawnBackroundAmbientChunkfish());
    }

    private IEnumerator SpawnBackroundAmbientChunkfish()
    {
        yield return new WaitForFixedUpdate();
        Transform cam = Manager_Cinemachine.instance.currentCinemachine.transform;
        float yOffset = Manager_Cinemachine.instance.currentCinemachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize * 2;

        Vector2 chunkfishPos = new Vector3(cam.position.x, cam.position.y - yOffset);
        GameObject backroundChunkfishClone = Instantiate(backroundChunkfish, chunkfishPos, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(15f, 20f));
        StartCoroutine(SpawnBackroundAmbientChunkfish());
    }
}
