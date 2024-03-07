using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoudSpeaker : MonoBehaviour, IDialogueCommunicator
{
    [Header("References")]
    [SerializeField] public List<ScriptableObject_Dialogue> _dialoguePackages;
    [SerializeField] public int dialoguePackageIteration = 0;
    [SerializeField] private Collider2D _promptCollider;
    [SerializeField] private bool isSpeaking;

    [Header("Building Block References")]
    [SerializeField] private LoudSpeaker_Animator _loudSpeaker_Animator;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("Actions")]
    [SerializeField] public Action onDialoguePackageSent;

    public void OnDialogueStart()
    {
        _loudSpeaker_Animator.ChangeToSpeaking();
    }

    public void OnDialogueEnd()
    {
        _loudSpeaker_Animator.ChangeToIdle();
    }

    private List<Dialogue> CopyDialoguePrompt()
    {
        // Create a new list with deep copy of elements
        List<Dialogue> _newDialogues = new List<Dialogue>();
        foreach (Dialogue dialogue in _dialoguePackages[dialoguePackageIteration]._dialogues)
        {
            // Dialogue class has a copy method to create a deep copy
            Dialogue newDialogue = new Dialogue(dialogue);
            _newDialogues.Add(newDialogue);
        }
        return _newDialogues;
    }

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

    private void SendInteraction()
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.isDialogueActive == false)
        {
            // For unique interactions
            onDialoguePackageSent?.Invoke();

            // Safety net
            _handler.currentDialoguePrompt = gameObject;

            // Copy it over
            _handler._dialogues = CopyDialoguePrompt();
            _handler._dialoguePackage = _dialoguePackages[dialoguePackageIteration];
            _handler.InitiateDialogue();

            // Next dialogue interaction
            if (dialoguePackageIteration < _dialoguePackages.Count - 1)
            {
                dialoguePackageIteration++;
            }
        }

    }
}
