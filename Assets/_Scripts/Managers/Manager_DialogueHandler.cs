using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Manager_DialogueHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject currentDialoguePrompt;

    [Header("Dialogue Pieces")]
    [SerializeField] private GameObject dialogueCanvas;

    [SerializeField] private GameObject current_dialogueSet;
    [SerializeField] private TextMeshProUGUI current_dialogueBoxText;
    [SerializeField] private GameObject current_profilePictureImage;
    [SerializeField] private TextMeshProUGUI current_profileNameTextMesh;

    // Top set
    [SerializeField] private GameObject top_dialogueSet;
    [SerializeField] private TextMeshProUGUI top_dialogueBoxText;
    [SerializeField] private GameObject top_profilePictureImage;
    [SerializeField] private TextMeshProUGUI top_profileNameTextMesh;

    // Top mini set
    [SerializeField] private GameObject topMini_dialogueSet;
    [SerializeField] private TextMeshProUGUI topMini_dialogueBoxText;
    [SerializeField] private GameObject topMini_profilePictureImage;

    // Bottom set
    [SerializeField] private GameObject bottom_dialogueSet;
    [SerializeField] private TextMeshProUGUI bottom_dialogueBoxText;
    [SerializeField] private GameObject bottom_profilePictureImage;
    [SerializeField] private TextMeshProUGUI bottom_profileNameTextMesh;

    [Header("Dialogue References")]
    [SerializeField] public List<Dialogue> _dialogues;
    [SerializeField] public ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] private int currentDialogueIndex;

    [SerializeField] public bool isDialogueActive;
    [SerializeField] private bool isDialogueTyping;
    [SerializeField] public bool isDialogueWaiting;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj _isDialoguePromptTag;


    [Header("Technical References")]
    [SerializeField] private PlayerInput playerInput = null;

    public static Manager_DialogueHandler instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Dialogue Handler Manager in the scene.");
        }
        instance = this;

        // Precaution
        dialogueCanvas.SetActive(false);

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

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        InteractPerformed();
    }

    public void SetCorrectDialoguePrompt()
    {
        Transform[] dialoguePromptPositions = FindAllDialoguePrompts();
        Transform playerPosition = Manager_PlayerState.instance.player.transform;

        currentDialoguePrompt = FindClosestDialoguePrompt(dialoguePromptPositions, playerPosition);
    }

    private Transform[] FindAllDialoguePrompts()
    {
        List<Transform> allTransforms = new List<Transform>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.TryGetComponent<Tags>(out var _tags))
            {
                if (_tags.CheckTags(_isDialoguePromptTag.name) == true)
                {
                    allTransforms.Add(obj.transform);
                }
            }
        }

        return allTransforms.ToArray();
    }

    private GameObject FindClosestDialoguePrompt(Transform[] tDialoguePrompts, Transform tPlayer)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity; // Any number compared will be smaller

        Vector2 currentPos = new Vector2(tPlayer.transform.position.x, tPlayer.transform.position.y);
        foreach (Transform t in tDialoguePrompts)
        {
            float dist = Vector2.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin.gameObject;
    }

    private void InteractPerformed()
    {
        // Speeds up dialogue if currently typing
        if (isDialogueTyping)
        {
            SkipDialogue();

        }
        // Wait next user input, hierarchy is important, must be below dialogue skipper
        if (isDialogueWaiting && !isDialogueTyping)
        {
            ProcessThroughDialogue(true);
        }
        else if (!isDialogueTyping)
        {
            ProcessThroughDialogue(false);
        }
    }

    public void SkipDialogue()
    {
        int i = currentDialogueIndex - 1;

        while (i < _dialogues.Count)
        {
            _dialogues[i].dialogueSpeed = 0.001f;
            _dialogues[i].dialogueStallTime = 0f;

            // Only skip text up to a stopping flag
            if (_dialogues[i].stoppingFlag)
            {
                break;
            }

            i++;
        }
    }

    private IEnumerator DelayInteractSubscription()
    {
        yield return new WaitForFixedUpdate();
        playerInput.Interact.Interact1.performed += OnInteractPerformed;
    }

    public void InitiateDialogue()
    {
        // Delayed interaction
        StopAllCoroutines();
        StartCoroutine(DelayInteractSubscription());

        // Player can't move until dialogue is over
        if (_dialoguePackage.isPlayerInputStall)
        {
            Manager_PlayerState.instance.SetInputStall(false);
        }

        // Relay signal
        if (currentDialoguePrompt != null)
        {
            if (currentDialoguePrompt.GetComponent<IDialogueCommunicator>() != null)
            {
                currentDialoguePrompt.GetComponent<IDialogueCommunicator>().OnDialogueStart();
            }
        }

        // Format where the dialogues are
        ProcessDialogueFormatting();

        // Technicals
        current_dialogueSet.SetActive(true);
        isDialogueActive = true;
        isDialogueTyping = false;
        isDialogueWaiting = false;
        dialogueCanvas.SetActive(true);
        currentDialogueIndex = 0;

        InteractPerformed();
    }

    private void ProcessDialogueFormatting()
    {
        // Normal top
        if (_dialoguePackage.isDialogueOnTop && _dialoguePackage.isDialogueMini == false)
        {
            current_dialogueSet = top_dialogueSet;
            current_dialogueBoxText = top_dialogueBoxText;
            current_profilePictureImage = top_profilePictureImage;
            current_profileNameTextMesh = top_profileNameTextMesh;
        }

        // Small top
        if (_dialoguePackage.isDialogueOnTop && _dialoguePackage.isDialogueMini == true)
        {
            current_dialogueSet = topMini_dialogueSet;
            current_dialogueBoxText = topMini_dialogueBoxText;
            current_profilePictureImage = topMini_profilePictureImage;
            current_profileNameTextMesh = null;
        }

        // Normal bottom
        if (_dialoguePackage.isDialogueOnTop == false && _dialoguePackage.isDialogueMini == false)
        {
            current_dialogueSet = bottom_dialogueSet;
            current_dialogueBoxText = bottom_dialogueBoxText;
            current_profilePictureImage = bottom_profilePictureImage;
            current_profileNameTextMesh = bottom_profileNameTextMesh;
        }
    }

    private void ProcessThroughDialogue(bool bypassStoppingFlag = false)
    {
        // Waits for user input if stopping flag is true
        if (currentDialogueIndex > 0) {
            if (_dialogues[currentDialogueIndex - 1].stoppingFlag == true && bypassStoppingFlag == false)
            {
                isDialogueWaiting = true;
                return;
            }
        }

        // Changes profile pic
        if (currentDialogueIndex < _dialogues.Count)
        {
            if (_dialogues[currentDialogueIndex].profilePicture != null)
            {
                current_profilePictureImage.GetComponent<Image>().sprite = _dialogues[currentDialogueIndex].profilePicture;
            }
        }

        // Changes profile name
        if (currentDialogueIndex < _dialogues.Count)
        {
            if (_dialogues[currentDialogueIndex].profileName != "" && current_profileNameTextMesh != null)
            {
                current_profileNameTextMesh.text = _dialogues[currentDialogueIndex].profileName;
            }
        }

        IterateThroughDialogue();
    }

    private void IterateThroughDialogue(bool bypassStoppingFlag = false)
    {
        if (currentDialogueIndex < _dialogues.Count)
        {
            // Refresh dialogue box text
            if (_dialogues[currentDialogueIndex].cleanText) { current_dialogueBoxText.text = ""; }

            // Start typing next
            StartCoroutine(TypeCharacters(_dialogues[currentDialogueIndex]));
            currentDialogueIndex++;
        } else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeCharacters(Dialogue dialogue)
    {
        isDialogueTyping = true;

        char[] characters = dialogue.dialogueText.ToCharArray();

        foreach (char c in characters)
        {
            current_dialogueBoxText.text += c;
            if (dialogue.dialogueSpeed > 0)
            {
                yield return new WaitForSeconds(dialogue.dialogueSpeed);
            }
        }

        yield return new WaitForSeconds(dialogue.dialogueStallTime);

        isDialogueTyping = false;

        ProcessThroughDialogue();
    }

    private void EndDialogue()
    {
        StopAllCoroutines();

        // Unsubcribe for interaction
        playerInput.Interact.Interact1.performed -= OnInteractPerformed;

        // Resume player input
        Manager_PlayerState.instance.SetInputStall(true);

        // Relay signal
        if (currentDialoguePrompt != null)
        {
            if (currentDialoguePrompt.GetComponent<IDialogueCommunicator>() != null)
            {
                currentDialoguePrompt.GetComponent<IDialogueCommunicator>().OnDialogueEnd();
            }
        }

        // End relay signal
        currentDialoguePrompt = null;

        // Technicals
        current_dialogueSet.SetActive(false);
        isDialogueWaiting = false;
        isDialogueTyping = false;
        dialogueCanvas.SetActive(false);
        currentDialogueIndex = 0;

        // To make sure no infinite dialogue loops occur
        StartCoroutine(DelayedDialogueEnd());
    }

    public void ForceQuitDialogue()
    {
        if (isDialogueActive)
        {
            EndDialogue();
        }
    }

    private IEnumerator DelayedDialogueEnd()
    {
        yield return new WaitForFixedUpdate();
        isDialogueActive = false;
    }
}
