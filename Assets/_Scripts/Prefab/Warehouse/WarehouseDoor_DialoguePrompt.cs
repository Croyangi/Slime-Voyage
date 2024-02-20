using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseDoor_DialoguePrompt : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private Collider2D col_prompt;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    public void ForceQuitOutOfRange()
    {
        Manager_DialogueHandler _handler = Manager_DialogueHandler.instance;

        if (_handler.currentDialoguePrompt == gameObject && _handler != null)
        {
            _handler.ForceQuitDialogue();
        }
    }

    public bool CheckExistingObjects()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(col_prompt, new ContactFilter2D(), colliders);

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

    public void OnDialogueStart()
    {
    }

    public void OnDialogueEnd()
    {
    }

    public void SendInteraction()
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
