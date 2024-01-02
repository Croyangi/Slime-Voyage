using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Manager_DialogueHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject currentDialoguePrompt;
    [SerializeField] public List<Dialogue> _dialogues;
    [SerializeField] private TextMeshProUGUI _dialogueBoxText;
    [SerializeField] private int currentDialogueIndex;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] public bool isDialogueActive;
    [SerializeField] private bool isDialogueTyping;
    [SerializeField] public bool isDialogueExiting;

    public static Manager_DialogueHandler instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Dialogue Handler Manager in the scene.");
        }
        instance = this;

        // Precaution
        SetDialogueBox();
    }

    private void SetDialogueBox()
    {
        dialogueBox.SetActive(isDialogueActive);
    }

    public void SkipDialogue()
    {
        int i = currentDialogueIndex - 1;

        while (true && i < _dialogues.Count)
        {
            _dialogues[i].dialogueSpeed = 0f;
            _dialogues[i].dialogueStallTime = 0f;

            // Only skip text up to a stopping flag
            if (_dialogues[i].stoppingFlag)
            {
                break;
            }

            i++;
        }
    }

    public void InitiateDialogue()
    {
        // Player can't move until dialogue is over
        Manager_PlayerState.instance.SetInputStall(false);

        // Relay signal
        currentDialoguePrompt.GetComponentInChildren<DialoguePrompt_Effects>().OnDialogueStart();
        currentDialoguePrompt.GetComponent<DialoguePrompt>().OnDialogueStart();

        // Technicals
        StopAllCoroutines();
        isDialogueActive = true;
        SetDialogueBox();

        currentDialogueIndex = 0;
        IterateThroughDialogue();
    }

    public void ContinueDialogue()
    {
        if (isDialogueTyping)
        {
            SkipDialogue();
        } else if (isDialogueExiting)
        {
            EndDialogue();
        } else
        {
            IterateThroughDialogue(true);
        }
    }

    private void IterateThroughDialogue(bool bypassStoppingFlag = false)
    {
        if (currentDialogueIndex < _dialogues.Count)
        {
            bool stoppingFlag = _dialogues[currentDialogueIndex].stoppingFlag;
            if (bypassStoppingFlag) { stoppingFlag = false; }

            if (!stoppingFlag)
            {
                // Refresh dialogue box text
                if (_dialogues[currentDialogueIndex].cleanText) { _dialogueBoxText.text = ""; }

                // Start typing next
                StartCoroutine(TypeCharacters(_dialogues[currentDialogueIndex]));
                currentDialogueIndex++;
            }
        } else
        {
            isDialogueExiting = true;
        }
    }

    private IEnumerator TypeCharacters(Dialogue dialogue)
    {
        isDialogueTyping = true;

        char[] characters = dialogue.dialogueText.ToCharArray();

        foreach (char c in characters)
        {
            _dialogueBoxText.text += c;
            if (dialogue.dialogueSpeed > 0)
            {
                yield return new WaitForSeconds(dialogue.dialogueSpeed);
            }
        }

        yield return new WaitForSeconds(dialogue.dialogueStallTime);

        isDialogueTyping = false;

        IterateThroughDialogue();
    }

    private void EndDialogue()
    {
        // Resume player input
        Manager_PlayerState.instance.SetInputStall(true);

        // Relay signal
        currentDialoguePrompt.GetComponent<DialoguePrompt>().OnDialogueEnd();
        currentDialoguePrompt.GetComponentInChildren<DialoguePrompt_Effects>().OnDialogueEnd();

        // Technicals
        StopAllCoroutines();
        isDialogueActive = false;
        isDialogueExiting = false;
        SetDialogueBox();
    }
}
