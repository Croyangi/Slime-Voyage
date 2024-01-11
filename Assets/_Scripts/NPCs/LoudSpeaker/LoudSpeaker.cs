using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoudSpeaker : MonoBehaviour, IDialogueCommunicator
{
    [Header("References")]
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private bool isSpeaking;

    [Header("Building Block References")]
    [SerializeField] private LoudSpeaker_Animator _loudSpeaker_Animator;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                SendInteraction();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                ForceQuitOutOfRange();
            }
        }
    }

    private void ForceQuitOutOfRange()
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.isDialogueActive == true && _handler.currentDialoguePrompt == gameObject)
        {
            _handler.EndDialogue();
        }
    }

    public void OnDialogueStart()
    {
        _loudSpeaker_Animator.ChangeToSpeaking();
    }

    public void OnDialogueEnd()
    {
        _loudSpeaker_Animator.ChangeToIdle();
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
