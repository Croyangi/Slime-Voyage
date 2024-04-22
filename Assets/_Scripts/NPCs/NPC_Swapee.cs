using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Swapee : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D col_detect;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("State References")]
    [SerializeField] private string currentState;

    const string SWAPEE_IDLE = "Swapee_Idle";
    const string SWAPEE_CRAZY = "Swapee_Crazy";

    private void Awake()
    {
        ChangeAnimationState(SWAPEE_IDLE);
    }

    private void OnDialogueStart()
    {
        ChangeAnimationState(SWAPEE_CRAZY);
    }

    private void OnDialogueEnd()
    {
        ChangeAnimationState(SWAPEE_IDLE);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        _animator.Play(newState);
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
                OnDialogueEnd();
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
