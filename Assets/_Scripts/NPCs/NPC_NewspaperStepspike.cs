using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_NewspaperStepspike : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialoguePrompt _dialoguePrompt;
    [SerializeField] private DialoguePrompt_Effects _dialoguePrompt_Effects;
    [SerializeField] private ScriptableObject_Dialogue _elevatorStallDialoguePackage;
    [SerializeField] private ScriptableObject_Dialogue _elevatorFallDialoguePackage;

    [Header("General References")]
    [SerializeField] private Animator _animator;

    [Header("Tags")]
    [SerializeField] private TagsScriptObj tag_player;

    [Header("State References")]
    [SerializeField] private string currentState;

    const string NEWSPAPER_PUTDOWNNEWSPAPER = "NewspaperStepspike_PutDownNewspaper";
    const string NEWSPAPER_PUTUPNEWSPAPER = "NewspaperStepspike_PutUpNewspaper";

    private void Awake()
    {
        ChangeAnimationState(NEWSPAPER_PUTUPNEWSPAPER);
    }

    private void Start()
    {
        Manager_DialogueHandler.instance.onDialogueStart += OnDialogueStart;
        Manager_DialogueHandler.instance.onDialogueEnd += OnDialogueEnd;
    }

    private void OnDialogueStart()
    {
        ChangeAnimationState(NEWSPAPER_PUTDOWNNEWSPAPER);
    }

    private void OnDialogueEnd()
    {
        ChangeAnimationState(NEWSPAPER_PUTUPNEWSPAPER);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        _animator.Play(newState);
    }

    public void OnElevatorStallDialogueInteraction()
    {
        Manager_DialogueHandler.instance.ForceQuitDialogue();
        _dialoguePrompt.dialoguePackageIteration = 0;
        _dialoguePrompt._dialoguePackages.Clear();
        _dialoguePrompt._dialoguePackages.Add(_elevatorStallDialoguePackage);
        _dialoguePrompt_Effects.ColorInnerCircle();
    }

    public void OnElevatorFallDialogueInteraction()
    {
        Manager_DialogueHandler.instance.ForceQuitDialogue();
        _dialoguePrompt.dialoguePackageIteration = 0;
        _dialoguePrompt._dialoguePackages.Clear();
        _dialoguePrompt._dialoguePackages.Add(_elevatorFallDialoguePackage);
        _dialoguePrompt_Effects.ColorInnerCircle();
    }
}
