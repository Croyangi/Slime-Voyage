using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction_Breakdance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialoguePrompt _dialoguePrompt;
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;
    [SerializeField] private ScriptableObject_Dialogue _newDialoguePackage;

    [SerializeField] private Collider2D col_detect;
    [SerializeField] private bool isDetected;
    [SerializeField] private GameObject slime;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                isDetected = true;
                slime = collision.gameObject;
                StartCoroutine(CheckBreakdance());
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

    private IEnumerator CheckBreakdance()
    {
        if (slime.GetComponentInChildren<BaseSlime_AnimatorHelper>().currentState == "BaseSlime_Emote_Spin")
        {
            TriggerDialogueInteraction();
        }

        yield return new WaitForFixedUpdate();
        if (isDetected)
        {
            StartCoroutine(CheckBreakdance());
        }
    }

    private void TriggerDialogueInteraction()
    {
        _dialoguePrompt._dialoguePackages.Clear();
        _dialoguePrompt._dialoguePackages.Add(_newDialoguePackage);
        _dialoguePrompt_Effects.ColorInnerCircle();
        isDetected = false;
        gameObject.SetActive(false);
    }
}
