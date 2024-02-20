using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader_Warehouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image closingTransition;
    [SerializeField] private GameObject backroundChunkfish;
    [SerializeField] private AudioClip audioClip_music;

    private void Awake()
    {
        SceneManager.LoadScene("Bootloader_DevTools", LoadSceneMode.Additive);
        SceneManager.LoadScene("Bootloader_Global", LoadSceneMode.Additive);

        StartCoroutine(SpawnBackroundAmbientChunkfish());

        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_music, transform, 1, true, Manager_AudioMixer.instance.mixer_music);
    }

    private void FadeOpenTransition()
    {
        closingTransition.color = new Color(0f, 0f, 0f, 1f);
        LeanTween.color(closingTransition.rectTransform, new Color(0f, 0f, 0f, 0f), 1f).setEaseInCubic();
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