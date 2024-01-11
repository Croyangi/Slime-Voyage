using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudSpeaker_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;

    [Header("State References")]
    [SerializeField] private string currentState;
    [SerializeField] private int currentPriority;
    [SerializeField] private float currentPriorityTime;

    const string NPC_LOUDSPEAKER_IDLE = "NPC_LoudSpeaker_Idle";
    const string NPC_LOUDSPEAKER_SPEAKING = "NPC_LoudSpeaker_Speaking";

    private void Awake()
    {
        ChangeAnimationState(NPC_LOUDSPEAKER_IDLE);
    }

    private void ChangeAnimationState(string newState, int newPriority = 0)
    {
        if (currentState == newState)
        {
            return;
        }

        if (currentPriority > newPriority && currentPriorityTime > 0)
        {
            return;
        }

        _animator.Play(newState);
        currentState = newState;
        currentPriority = newPriority;
    }

    public void ChangeToIdle()
    {
        ChangeAnimationState(NPC_LOUDSPEAKER_IDLE);
    }

    public void ChangeToSpeaking()
    {
        ChangeAnimationState(NPC_LOUDSPEAKER_SPEAKING);
    }
}
