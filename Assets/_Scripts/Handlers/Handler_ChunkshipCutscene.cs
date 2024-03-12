using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Handler_ChunkshipCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject stepspike;
    [SerializeField] private GameObject easterEggText;

    [SerializeField] private GameObject fakeBaseSlime;
    [SerializeField] private Animator fakeBaseSlime_animator;

    [SerializeField] private Transform dropOffPoint;
    [SerializeField] private Transform flyOffPoint;

    [SerializeField] private GameObject cinemachine;
    [SerializeField] private float zoomInTransitionMultiplier = 1;
    [SerializeField] private float cameraZoom = 10f;

    [SerializeField] private GameObject chunkship;
    [SerializeField] private GameObject chunkship_top;
    [SerializeField] private GameObject chunkship_bottom;
    [SerializeField] private Vector2 chunkship_offset;

    [SerializeField] private AudioClip audioClip_breakingProtocol;

    [Header("Rise/Fall Visual Settings")]
    [SerializeField] private float amplitudeY = 0;
    [SerializeField] private float frequencyY = 1;
    [SerializeField] private float bottom_amplitudeRotate = 0;
    [SerializeField] private float bottom_frequencyRotate = 1;
    [SerializeField] private float top_amplitudeRotate = 0;
    [SerializeField] private float top_frequencyRotate = 1;
    [SerializeField] private float time;

    const string BASESLIME_IDLE = "BaseSlime_Idle";

    [ContextMenu("Initiate Checkscene")]
    public void InitiateCheckpointCutscene(GameObject checkpoint)
    {
        cinemachine.SetActive(true);

        dropOffPoint = checkpoint.transform;

        cinemachine.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 4f, cinemachine.transform.position.z);
        cinemachine.SetActive(true);

        InitiateChunkshipVFX();
    }

    private void InitiateChunkshipVFX()
    {
        // Set up Chunkfish
        fakeBaseSlime_animator.Play(BASESLIME_IDLE);
        StartCoroutine(ChunkshipVFXLoop());
        chunkship.transform.position = (Vector2) cinemachine.transform.position + chunkship_offset;

        StartCoroutine(InitiateChunkshipDocking());
    }

    private IEnumerator ChunkshipVFXLoop()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time * frequencyY) * amplitudeY;
        float top_rotateZ = Mathf.Sin(time * top_frequencyRotate) * top_amplitudeRotate;
        float bottom_rotateZ = Mathf.Sin(time * bottom_frequencyRotate) * bottom_amplitudeRotate;
        chunkship.transform.position = new Vector2(chunkship.transform.position.x, chunkship.transform.position.y + y);
        chunkship_top.transform.rotation = Quaternion.Euler(new Vector3(0, 0, top_rotateZ));
        chunkship_bottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bottom_rotateZ));

        yield return new WaitForFixedUpdate();
        StartCoroutine(ChunkshipVFXLoop());
    }

    private IEnumerator InitiateChunkshipDocking()
    {
        yield return new WaitForSeconds(2f);

        LeanTween.moveX(chunkship, dropOffPoint.position.x - 1f, 5f).setEaseInOutSine();
        LeanTween.moveY(chunkship, dropOffPoint.position.y + 5.5f, 5f).setEaseInOutSine();

        yield return new WaitForSeconds(2f);
        StartCoroutine(ChunkshipDockingLoop());

        yield return new WaitForSeconds(3f);
        StartCoroutine(ChunkshipUndocking());
    }

    private IEnumerator ChunkshipDockingLoop()
    {
        float size = cinemachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        cinemachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = Mathf.Lerp(size, cameraZoom, Time.deltaTime * zoomInTransitionMultiplier); // Lerp is NOT exponential, not self-referenced value

        yield return new WaitForFixedUpdate();
        StartCoroutine(ChunkshipDockingLoop());
    }

    private IEnumerator ChunkshipUndocking()
    {
        SlimeDropOff();
        fakeBaseSlime.SetActive(false);

        yield return new WaitForSeconds(1f);

        float multiplier = 4f;
        float time = 3f;
        LeanTween.moveX(chunkship, dropOffPoint.position.x + -(chunkship_offset.x) * multiplier, time * multiplier).setEaseInOutSine();
        LeanTween.moveY(chunkship, dropOffPoint.position.y + chunkship_offset.y * multiplier, time * multiplier).setEaseInOutSine();
        yield return new WaitForSeconds(time * multiplier);
        easterEggText.SetActive(true);
        yield return new WaitForSeconds(4f);
        CutsceneCleanup();
    }

    private void SlimeDropOff()
    {
        // Stepspike Push
        stepspike.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        stepspike.GetComponent<Rigidbody2D>().AddForce(new Vector2(-7, 15f), ForceMode2D.Impulse);
        stepspike.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-300f, -200f);
        Destroy(stepspike, 5f);

        // Base Slime stuff
        GameObject baseSlime = Manager_PlayerState.instance.player;
        baseSlime.transform.position = fakeBaseSlime.transform.position;
        baseSlime.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        baseSlime.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 20f), ForceMode2D.Impulse);

        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_breakingProtocol, transform, 0.5f, true, Manager_AudioMixer.instance.mixer_music);

        // Cleanup
        cinemachine.SetActive(false);
    }

    private void CutsceneCleanup()
    {
        // Cleanup
        chunkship.SetActive(false);
        StopAllCoroutines();
    }
}
