using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguePrompt : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Dialogue> _dialogues;

    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

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
        playerInput.Interact.Interact1.performed += OnInteractPerformed;
    }

    public void OnDialogueEnd()
    {
    }

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.isDialogueActive)
        {
            _handler.ContinueDialogue();

        } else
        {
            _handler.currentDialoguePrompt = gameObject;

            // Create new memory space, since lists are reference types
            //List<Dialogue> _newDialogues = new List<Dialogue>();
            //_dialogues.CopyTo(_newDialogues.ToArray());

            _handler._dialogues = new List<Dialogue>(_dialogues);
            _handler.InitiateDialogue();
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
            }
        }
    }
}
