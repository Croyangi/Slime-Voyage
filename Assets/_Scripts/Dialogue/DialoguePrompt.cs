using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguePrompt : MonoBehaviour, IDialogueCommunicator
{
    [Header("References")]
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private Collider2D _promptCollider;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    [Header("Building Block References")]
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _playerTag;

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

        if (_handler.isDialogueActive == false)
        {
            // Find nearest dialogue
            _handler.SetCorrectDialoguePrompt();

            if (_handler.currentDialoguePrompt == gameObject)
            {
                // Safety net
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
