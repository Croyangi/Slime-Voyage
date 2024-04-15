using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction_ChunkfishBouncer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialoguePrompt _dialoguePrompt;
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;
    [SerializeField] private ScriptableObject_Dialogue _newDialoguePackage;
    [SerializeField] private bool isDetected;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                isDetected = true;
                TriggerDialogueInteraction();
            }
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
