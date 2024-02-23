using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteraction_DestroyWarehouseBoxes : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialoguePrompt _dialoguePrompt;
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private ScriptableObject_Dialogue _newDialoguePackage;

    [SerializeField] private Collider2D col_detect;
    [SerializeField] private bool isDetected;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    private void OnEnable()
    {
        //_dialoguePrompt.onDialoguePackageSent += InitiateCheckBrokenBoxes;
    }

    private void OnDestroy()
    {
        //_dialoguePrompt.onDialoguePackageSent -= InitiateCheckBrokenBoxes;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true)
            {
                isDetected = true;
                InitiateCheckBrokenBoxes();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tags>(out var _tags))
        {
            if (_tags.CheckTags(tag_player.name) == true && CheckExistingObjects() == false)
            {
                isDetected = false;
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

    private void InitiateCheckBrokenBoxes()
    {
        StartCoroutine(CheckBrokenBoxes());
    }

    private IEnumerator CheckBrokenBoxes()
    {
        foreach (GameObject box in boxes)
        {
            if (box == null) 
            {
                if (_dialoguePrompt.dialoguePackageIteration == 0)
                {
                    _dialoguePrompt._dialoguePackages.Clear();
                }
                _dialoguePrompt._dialoguePackages.Add(_newDialoguePackage);
                _dialoguePrompt_Effects.ColorInnerCircle();
                isDetected = false;
                yield break;
            }
        }
        yield return new WaitForFixedUpdate();
        if (isDetected)
        {
            StartCoroutine(CheckBrokenBoxes());
        }
    }
}
