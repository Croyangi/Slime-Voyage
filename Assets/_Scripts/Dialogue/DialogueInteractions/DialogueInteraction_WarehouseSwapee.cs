using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction_WarehouseSwapee : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject idleGroup;
    [SerializeField] private GameObject destroyedGroup;

    [SerializeField] private Collider2D col_detect;
    [SerializeField] private bool isDetected;
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;

    [SerializeField] private AudioClip audioClip_forkliftExplosion;
    [SerializeField] private AudioClip audioClip_forkliftFire;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("State References")]
    [SerializeField] private string currentState;

    const string NPC_FORKLIFT_IDLE = "NPC_Forklift_Idle";
    const string NPC_FORKLIFT_DESTROYED = "NPC_Forklift_Destroyed";

    private void Awake()
    {
        ChangeAnimationState(NPC_FORKLIFT_IDLE);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        _animator.Play(newState);
    }

    [ContextMenu("Change To Destoryed")]
    public void ChangeToDestroyed()
    {
        ChangeAnimationState(NPC_FORKLIFT_DESTROYED);
        idleGroup.SetActive(false);
        destroyedGroup.SetActive(true);

        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_forkliftExplosion, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx, false, 0f, 1f, 1f, 30f);
        Manager_SFXPlayer.instance.PlaySFXClip(audioClip_forkliftFire, transform, 0.3f, true, Manager_AudioMixer.instance.mixer_sfx, true, 0.2f, 1f, 1f, 30f);
    }

    private IEnumerator LoopingCheckForDestroyedForklift()
    {
        if (Manager_DialogueHandler.instance.isDialogueActive && Manager_DialogueHandler.instance.currentDialogueIndex >= 12 && Manager_DialogueHandler.instance._dialoguePackage == _dialoguePackage)
        {
            ChangeToDestroyed();
            isDetected = false;

            StopAllCoroutines();
        }

        if (isDetected)
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(LoopingCheckForDestroyedForklift());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                isDetected = true;
                StartCoroutine(LoopingCheckForDestroyedForklift());
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
