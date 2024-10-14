using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Handler_OminousEmployerIntro : MonoBehaviour, IDataPersistence
{
    [SerializeField] private AudioClip sfx_ominousEmployerVoice0;
    [SerializeField] private AudioClip sfx_ominousEmployerVoice1;
    [SerializeField] private AudioClip sfx_lightStringPull;

    [Header("Technical")]
    [SerializeField] private PlayerInput playerInput = null;
    public Action<bool> NoddingCallback;
    [SerializeField] private Handler_OminousEmployerIntroCamera _ominousCamera;

    [Header("Cutscene Pieces")]
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject fan;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueContinue;

    [SerializeField] private List<Dialogue> allDialogueSets;
    [SerializeField] private string currentDialogueYes;
    [SerializeField] private string currentDialogueNo;

    [SerializeField] private string dialogueSet0Yes;
    [SerializeField] private string dialogueSet0No;
    [SerializeField] private string dialogueSet1Yes;
    [SerializeField] private string dialogueSet1No;
    [SerializeField] private string dialogueSet2Yes;
    [SerializeField] private string dialogueSet2No;
    [SerializeField] private string dialogueSet3Yes;
    [SerializeField] private string dialogueSet3No;


    [SerializeField] private GameObject cutsceneObjects;
    [SerializeField] private TextMeshProUGUI tmp_text;
    [SerializeField] private TypewriterByCharacter typewriter;
    [SerializeField] private int textIndex = 0;
    [SerializeField] private int dialogueIndex = 0;
    [SerializeField] private bool isSkippable;
    [SerializeField] private bool isDetectingResponse;
    [SerializeField] private bool isWaitingResponse;
    [SerializeField] private bool isDetectingNodding;
    [SerializeField] private bool isDialogueStopped;
    [SerializeField] private bool isDialogueFinal;

    [Serializable]
    public class Dialogue
    {
        [TextArea]
        public List<string> dialogueSet;
    }

    private void Awake()
    {
        playerInput = new PlayerInput(); // Instantiate new Unity's Input System

        Cursor.visible = false;

        DisableDialogueContinue();
        cutsceneObjects.SetActive(false);
        typewriter.SetTypewriterSpeed(0.01f);

        StartCoroutine(CutsceneTechnicals());
    }

    private void OnEnable()
    {
        //// Subscribes to Unity's input system
        playerInput.Enable();
        playerInput.Interact.Interact1.performed += OnInteractPerformed;
        NoddingCallback += OnNoddingDetect;
    }

    private void OnDestroy()
    {
        //// Unubscribes to Unity's input system
        playerInput.Interact.Interact1.performed -= OnInteractPerformed;
        playerInput.Disable();
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {
        data.hasPlayedGameBefore = true;
    }

    public void OnCharacterShownSFX()
    {
        if (!isDialogueFinal)
        {
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_ominousEmployerVoice0, transform, 0.1f, isPitchShifted: true, pitchShift: 0.1f);
        } else
        {
            Manager_SFXPlayer.instance.PlaySFXClip(sfx_ominousEmployerVoice1, transform, 0.1f, isPitchShifted: true, pitchShift: 0.1f);
        }
    }

    private void OnNoddingDetect(bool isNoddingYes)
    {
        if (isNoddingYes)
        {
            typewriter.ShowText(currentDialogueYes);
        }
        else
        {
            typewriter.ShowText(currentDialogueNo);
        }
        typewriter.StartShowingText(true);

        isDetectingNodding = false;
        DisableDialogueContinue();
        isSkippable = true;
        isWaitingResponse = true;
        OnNewDialogueSet();
    }

    private void OnNewDialogueSet()
    {
        textIndex = 0;
        dialogueIndex++;
    }

    private void OnInteractPerformed(InputAction.CallbackContext value)
    {
        if (isDetectingResponse)
        {
            if (isSkippable)
            {
                OnSkipPerformed();
            }
            else if (!isDialogueStopped)
            {
                // Adds to textIndex AFTER passing parameters
                OnNextDialogue(textIndex++, allDialogueSets[dialogueIndex].dialogueSet);
            } else
            {
                tmp_text.text = "";
                isDialogueFinal = true;
                typewriter.SetTypewriterSpeed(0.001f);
            }
        }
    }

    private void OnNextDialogue(int index, List<string> dialogueTexts)
    {
        DisableDialogueContinue();
        isSkippable = true;
        isWaitingResponse = true;
        typewriter.ShowText(dialogueTexts[index % dialogueTexts.Count]);
        typewriter.StartShowingText(true);

    }

    private void OnSkipPerformed()
    {
        DisableDialogueContinue();
        typewriter.SkipTypewriter();
    }

    private IEnumerator CutsceneTechnicals()
    {
        yield return new WaitForSeconds(2);
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_lightStringPull, transform, 1f);
        cutsceneObjects.SetActive(true);

        yield return new WaitForSeconds(3);
        MoveUpPaper();

        yield return new WaitForSeconds(2);
        dialogueBox.SetActive(true);
        OnNextDialogue(textIndex++, allDialogueSets[dialogueIndex].dialogueSet);

        // Lot of spaghetti code for cutscene and nodding detect standby
        ///////////////
        currentDialogueYes = dialogueSet0Yes;
        currentDialogueNo = dialogueSet0No;
        while (!isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }
        _ominousCamera.InitiateNoddingDetect();
        while (isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }

        ///////////////
        currentDialogueYes = dialogueSet1Yes;
        currentDialogueNo = dialogueSet1No;
        while (!isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }
        _ominousCamera.InitiateNoddingDetect();
        while (isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }

        ///////////////
        currentDialogueYes = dialogueSet2Yes;
        currentDialogueNo = dialogueSet2No;
        while (!isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }
        _ominousCamera.InitiateNoddingDetect();
        while (isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }

        ///////////////
        currentDialogueYes = dialogueSet3Yes;
        currentDialogueNo = dialogueSet3No;
        while (!isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }
        _ominousCamera.InitiateNoddingDetect();
        isDialogueStopped = true;
        while (isDetectingNodding)
        {
            yield return new WaitForFixedUpdate();
        }


        while (!isDialogueFinal)
        {
            yield return new WaitForFixedUpdate();
        }
        Manager_SFXPlayer.instance.PlaySFXClip(sfx_lightStringPull, transform, 1f);
        cutsceneObjects.SetActive(false);
        dialogueBox.SetActive(false);
        fan.SetActive(false);

        yield return new WaitForSeconds(3f);
        dialogueBox.SetActive(true);
        OnNextDialogue(textIndex++, allDialogueSets[dialogueIndex].dialogueSet);

        DataPersistenceManager.instance.SaveGame();
        yield return new WaitForSeconds(5f);
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");

    }

    private void FixedUpdate()
    {
        if (isWaitingResponse && !typewriter.isShowingText)
        {
            isWaitingResponse = false;

            if (textIndex < allDialogueSets[dialogueIndex].dialogueSet.Count)
            {
                EnableDialogueContinue();
                isSkippable = false;
                isDetectingResponse = true;
            } else
            {
                isDetectingNodding = true;
            }
        }
    }

    private void EnableDialogueContinue()
    {
        LeanTween.scale(dialogueContinue, Vector3.one, 0.2f).setEaseOutBack();
    }

    private void DisableDialogueContinue()
    {
        LeanTween.scale(dialogueContinue, Vector3.zero, 0);
    }

    private void MoveUpPaper()
    {
        LeanTween.moveLocalY(paper, -1.79f, 2f).setEaseOutBack();
    }
}
