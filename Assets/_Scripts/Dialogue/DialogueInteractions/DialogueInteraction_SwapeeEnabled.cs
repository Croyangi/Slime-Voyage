using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DialogueInteraction_SwapeeEnabled : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Collider2D col_detect;
    [SerializeField] private bool isDetected;
    [SerializeField] private bool isCompletedDialogue;
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private Handler_WarehouseSwapeeMode _swapeeMode;
    [SerializeField] private Image fadeOut;

    [SerializeField] private AudioClip audioClip_swapeeGrowing;

    [Header("Swapee")]
    [SerializeField] private GameObject swapee;
    [SerializeField] private GameObject swapee_dialoguePrompt;
    [SerializeField] private ParticleSystem swapee_particles;
    [SerializeField] private float elapsedTime;
    [SerializeField] private Light2D swapee_light;
    [SerializeField] private GameObject swapee_globalVolume;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;


    [ContextMenu("Start Growing")]
    public IEnumerator SwapeeGrowing()
    {
        elapsedTime += Time.deltaTime;

        Vector3 scale = swapee.transform.localScale;
        float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 6f);

        swapee.transform.localScale = scale * 1.01f;

        swapee_light.intensity += 0.04f;
        fadeOut.color = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, alpha);
        
        yield return new WaitForFixedUpdate();
        StartCoroutine(SwapeeGrowing());
    }

    private IEnumerator InitiateSwapeeGrowing()
    {
        yield return new WaitForSeconds(3f);

        swapee_particles.Play();
        fadeOut.enabled = true;
        swapee_globalVolume.SetActive(true);
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_swapeeGrowing, transform, 1f, false, Manager_AudioMixer.instance.mixer_sfx);
        StartCoroutine(SwapeeGrowing());

        yield return new WaitForSeconds(7f);

        swapee.SetActive(false);
        swapee_particles.Stop();
        fadeOut.enabled = false;
        swapee_light.enabled = false;
        swapee_globalVolume.SetActive(false);

        StopAllCoroutines();
        _swapeeMode.EndSwapeeModeIntro();
    }

    private void OnDialogueEnd()
    {
        if (isCompletedDialogue)
        {
            swapee_dialoguePrompt.SetActive(false);
        }
    }

    private IEnumerator LoopingCheckForSwapee()
    {
        if (Manager_DialogueHandler.instance.isDialogueActive && Manager_DialogueHandler.instance.currentDialogueIndex >= 10 && Manager_DialogueHandler.instance._dialoguePackage == _dialoguePackage)
        {
            StartCoroutine(InitiateSwapeeGrowing());
            isDetected = false;
            isCompletedDialogue = true;
        }

        if (isDetected)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(LoopingCheckForSwapee());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                isDetected = true;
                StartCoroutine(LoopingCheckForSwapee());
                Manager_DialogueHandler.instance.onDialogueEnd += OnDialogueEnd;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && CheckExistingObjects() == false)
            {
                isDetected = false;
                Manager_DialogueHandler.instance.onDialogueEnd -= OnDialogueEnd;
            }
        }
    }

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_detect, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(tag_player.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
