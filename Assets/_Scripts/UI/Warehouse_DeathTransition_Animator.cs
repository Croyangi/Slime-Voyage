using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Warehouse_DeathTransition_Animator : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private Animator _animator;

    [Header("State References")]
    [SerializeField] private string currentState;

    const string DEATHTRANSITION_CLOSED = "Warehouse_DeathTransition_Close";
    const string DEATHTRANSITION_OPEN = "Warehouse_DeathTransition_Open";

    public void PlayDeathTransitionClose()
    {
        _animator.Play(DEATHTRANSITION_CLOSED);
    }

    public void PlayDeathTransitionOpen()
    {
        _animator.Play(DEATHTRANSITION_OPEN);
    }
}
