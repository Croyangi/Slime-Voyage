using System;
using System.Collections;
using System.Collections.Generic;
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

    [Serializable]
    public class DialogueSet
    {
        public GameObject set;
        public Vector3 setScale;

        public TextMeshProUGUI dialogueText;
        public GameObject dialogue;
        public Vector3 dialogueScale;

        public GameObject image;
        public GameObject dialogueImage;
        public Vector3 dialogueImageScale;

        public TextMeshProUGUI nameText;
        public GameObject name;
        public Vector3 nameScale;
    }

    [SerializeField] private List<DialogueSet> dialogueSets;
    [SerializeField] private int dialogueSetIndex = 0;
    // 0 is top set
    // 1 is top mini set
    // 2 is bottom set

    [SerializeField] private GameObject current_dialogueSet;
    [SerializeField] private TextMeshProUGUI current_dialogueBoxText;
    [SerializeField] private GameObject current_profilePictureImage;
    [SerializeField] private TextMeshProUGUI current_profileNameTextMesh;

    [Header("Dialogue References")]
    [SerializeField] public List<Dialogue> _dialogues;
    [SerializeField] public ScriptableObject_Dialogue _dialoguePackage;
    [SerializeField] public int currentDialogueIndex;
    [SerializeField] private AudioClip currentDialogueSpeakingSFX;

    [SerializeField] public bool isDialogueActive;
    [SerializeField] private bool isDialogueTyping;
    [SerializeField] public bool isDialogueWaiting;
    [SerializeField] private bool isDialogueSkipping;

    [SerializeField] private float currentDialogueSpeed;
    [SerializeField] private float currentDialogueStallTime;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_isDialoguePrompt;


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

        playerInput = new PlayerInput(); // Instantiate new Unity's Input System

        GetDialogueSetScales();
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

    private void GetDialogueSetScales()
    {
        foreach (DialogueSet d in dialogueSets)
        {
            if (d.set != null) { d.setScale = d.set.transform.localScale; }
            if (d.dialogue != null) { d.dialogueScale = d.dialogue.transform.localScale; }
            if (d.name != null) { d.nameScale = d.name.transform.localScale; }
            if (d.dialogueImage != null) { d.dialogueImageScale = d.dialogueImage.transform.localScale; }
        }
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
                if (_tags.CheckTags(tag_isDialoguePrompt.name) == true)
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
        isDialogueSkipping = true;

        while (i < _dialogues.Count)
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

        // Easily enable dialogue sets
        ProcessDialogueFormatting();
        DialogueScaleUp();
        current_dialogueSet.SetActive(true);

        // Technical starts
        isDialogueActive = true;
        isDialogueTyping = false;
        isDialogueWaiting = false;
        currentDialogueIndex = 0;

        InteractPerformed();
    }

    private void DialogueScaleUp()
    {
        if (current_dialogueBoxText != null)
        {
            GameObject text = current_dialogueBoxText.transform.parent.gameObject;
            text.transform.localScale = Vector3.zero;
            LeanTween.scale(text, dialogueSets[dialogueSetIndex].dialogueScale, 0.1f);
        }

        if (current_profilePictureImage != null)
        {
            GameObject image = current_profilePictureImage.transform.parent.gameObject;
            image.transform.localScale = Vector3.zero;
            LeanTween.scale(image, dialogueSets[dialogueSetIndex].dialogueImageScale, 0.1f);
        }

        if (current_profileNameTextMesh != null)
        {
            GameObject name = current_profileNameTextMesh.transform.parent.gameObject;
            name.transform.localScale = Vector3.zero;
            LeanTween.scale(name, dialogueSets[dialogueSetIndex].nameScale, 0.1f);
        }
    }

    private IEnumerator DialogueShrinkDown()
    {
        GameObject text = null;
        if (current_dialogueBoxText != null)
        {
            text = current_dialogueBoxText.transform.parent.gameObject;
            LeanTween.scale(text, Vector3.zero, 0.1f);
        }

        GameObject image = null;
        if (current_profilePictureImage != null)
        {
            image = current_profilePictureImage.transform.parent.gameObject;
            LeanTween.scale(image, Vector3.zero, 0.1f);
        }

        GameObject name = null;
        if (current_profileNameTextMesh != null)
        {
            name = current_profileNameTextMesh.transform.parent.gameObject;
            LeanTween.scale(name, Vector3.zero, 0.1f);
        }

        yield return new WaitForSeconds(0.2f);
        current_dialogueSet.SetActive(false);

        if (text != null)
        {
            text.transform.localScale = dialogueSets[dialogueSetIndex].dialogueScale;
        }

        if (image != null)
        {
            image.transform.localScale = dialogueSets[dialogueSetIndex].dialogueImageScale;
        }

        if (name != null)
        {
            name.transform.localScale = dialogueSets[dialogueSetIndex].nameScale;
        }
    }

    private void ProcessDialogueFormatting()
    {
        // Normal top
        if (_dialoguePackage.isDialogueOnTop && _dialoguePackage.isDialogueMini == false)
        {
            dialogueSetIndex = 0;
            current_dialogueSet = dialogueSets[0].set;
            current_dialogueBoxText = dialogueSets[0].dialogueText;
            current_profilePictureImage = dialogueSets[0].image;
            current_profileNameTextMesh = dialogueSets[0].nameText;
        }

        // Small top
        if (_dialoguePackage.isDialogueOnTop && _dialoguePackage.isDialogueMini == true)
        {
            dialogueSetIndex = 1;
            current_dialogueSet = dialogueSets[1].set;
            current_dialogueBoxText = dialogueSets[1].dialogueText;
            current_profilePictureImage = dialogueSets[1].image;
            current_profileNameTextMesh = null;
        }

        // Normal bottom
        if (_dialoguePackage.isDialogueOnTop == false && _dialoguePackage.isDialogueMini == false)
        {
            dialogueSetIndex = 2;
            current_dialogueSet = dialogueSets[2].set;
            current_dialogueBoxText = dialogueSets[2].dialogueText;
            current_profilePictureImage = dialogueSets[2].image;
            current_profileNameTextMesh = dialogueSets[2].nameText;
        }
    }

    private void ProcessThroughDialogue(bool bypassStoppingFlag = false)
    {
        // Waits for user input if stopping flag is true
        if (currentDialogueIndex > 0)
        {
            if (_dialogues[currentDialogueIndex - 1].stoppingFlag == true && bypassStoppingFlag == false)
            {
                isDialogueWaiting = true;
                isDialogueSkipping = false;
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

        // Changes speaking sfx
        if (currentDialogueIndex < _dialogues.Count)
        {
            if (_dialogues[currentDialogueIndex].sfx_speaking != null)
            {
                currentDialogueSpeakingSFX = _dialogues[currentDialogueIndex].sfx_speaking;
            }
        }

        IterateThroughDialogue();
    }

    private void IterateThroughDialogue()
    {
        if (currentDialogueIndex < _dialogues.Count)
        {
            // Refresh dialogue box text
            if (_dialogues[currentDialogueIndex].cleanText) { current_dialogueBoxText.text = ""; }

            // Start typing next
            StartCoroutine(TypeCharacters(_dialogues[currentDialogueIndex]));
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeCharacters(Dialogue dialogue)
    {
        isDialogueTyping = true;
        int sfxSpeakingSpacing = _dialogues[currentDialogueIndex].sfxSpeakingSpacing;
        int sfxSpeakingIteration = 0;

        char[] characters = dialogue.dialogueText.ToCharArray();

        foreach (char c in characters)
        {
            current_dialogueBoxText.text += c;

            // SFX for speaking
            if (currentDialogueSpeakingSFX != null && sfxSpeakingIteration > sfxSpeakingSpacing && isDialogueSkipping == false)
            {
                Manager_SFXPlayer.instance.PlaySFXClip(currentDialogueSpeakingSFX, transform, 0.3f, false, Manager_AudioMixer.instance.mixer_sfx);
                sfxSpeakingIteration = 0;
            } else if (isDialogueSkipping == false)
            {
                sfxSpeakingIteration++;
            }

            // Dialogue spacing
            if (dialogue.dialogueSpeed > 0)
            {
                currentDialogueSpeed = dialogue.dialogueSpeed;
                yield return new WaitForSeconds(dialogue.dialogueSpeed);
            }
        }

        currentDialogueStallTime = dialogue.dialogueStallTime;
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

        // UI Smooth Stuff
        StartCoroutine(DialogueShrinkDown());

        // Technicals
        isDialogueWaiting = false;
        isDialogueTyping = false;
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