using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SwapeeDemoEnd : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Collider2D col_detect;
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private GameObject dialoguePrompt;
    [SerializeField] private bool isCompleted;

    [SerializeField] private BootLoader_Basement _basement;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("Scene")]
    [SerializeField] private SceneQueue _sceneQueue;

    private void OnDialogueStart()
    {
        Manager_PlayerState.instance.SetResetDeath(false);
    }

    private void OnDialogueEnd()
    {
        if (Manager_DialogueHandler.instance._dialoguePackage == _dialoguePackage && isCompleted == false)
        {
            isCompleted = true;
            Manager_PlayerState.instance.SetInputStall(false);
            dialoguePrompt.SetActive(false);

            _basement.OnBasementComplete();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                Manager_DialogueHandler.instance.onDialogueStart += OnDialogueStart;
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
                Manager_DialogueHandler.instance.onDialogueStart -= OnDialogueStart;
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
