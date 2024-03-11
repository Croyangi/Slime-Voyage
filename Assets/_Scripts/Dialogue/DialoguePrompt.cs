using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguePrompt : MonoBehaviour, IDialogueCommunicator
{
    [Header("References")]
    [SerializeField] public List<ScriptableObject_Dialogue> _dialoguePackages;
    [SerializeField] public int dialoguePackageIteration = 0;
    [SerializeField] private Collider2D _promptCollider;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

    [Header("Actions")]
    [SerializeField] public Action onDialoguePackageSent;

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Enable();
    }

    private void OnDestroy()
    {
        //// Unubscribes to Unity's input system
        playerInput.Interact.Interact1.performed -= OnInteractPerformed;
        playerInput.Disable();
    }

    public void OnDialogueStart()
    {
        playerInput.Interact.Interact1.performed -= OnInteractPerformed;
        _dialoguePrompt_Effects.OnDialogueStart();
    }

    public void OnDialogueEnd()
    {
        if (CheckExistingObjects() == true)
        {
            playerInput.Interact.Interact1.performed += OnInteractPerformed;
            _dialoguePrompt_Effects.OnDialogueEnd();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.isDialogueActive == false) ///////////
        {
            // For unique interactions
            onDialoguePackageSent?.Invoke();

            // Find nearest dialogue
            _handler.SetCorrectDialoguePrompt();

            if (_handler.currentDialoguePrompt == gameObject)
            {
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
                else
                {
                    _dialoguePrompt_Effects.GrayOutInnerCircle();
                }
            }
        } ///////////

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
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                playerInput.Interact.Interact1.performed += OnInteractPerformed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(_playerTag.name) == true)
            {
                playerInput.Interact.Interact1.performed -= OnInteractPerformed;
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

    private bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(_promptCollider, new ContactFilter2D(), colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(_playerTag.name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
