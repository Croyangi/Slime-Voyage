using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                SendInteraction();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                ForceQuitOutOfRange();
            }
        }
    }

    private void ForceQuitOutOfRange()
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.currentDialoguePrompt == gameObject && _handler != null)
        {
            _handler.ForceQuitDialogue();
        }
    }

    public void OnDialogueStart()
    {
    }

    public void OnDialogueEnd()
    {
    }

    private void SendInteraction()
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.isDialogueActive == false)
        {
            _handler.currentDialoguePrompt = gameObject;

            // Create a new list with deep copy of elements
            List<Dialogue> _newDialogues = new List<Dialogue>();
            foreach (Dialogue dialogue in _dialoguePackage._dialogues)
            {
                // Dialogue class has a copy method to create a deep copy
                Dialogue newDialogue = new Dialogue(dialogue);
                _newDialogues.Add(newDialogue);
            }

            // Stupid referenced memory >:(
            _handler._dialogues = _newDialogues;
            _handler._dialoguePackage = _dialoguePackage;
            _handler.InitiateDialogue();
        }

    }
}
