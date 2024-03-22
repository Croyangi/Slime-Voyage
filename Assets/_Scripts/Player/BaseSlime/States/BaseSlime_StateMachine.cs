using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseSlime_StateMachine : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle, Moving, Jumping, Airborne, Sticking, OnEdge, Compressed, LookingUp
    }

    [SerializeField] public PlayerStates playerStates;
    [SerializeField] private State currentState;
    [SerializeField] private BaseSlime_IdleState idle;
    [SerializeField] private BaseSlime_MovingState moving;
    [SerializeField] private BaseSlime_AirborneState airborne;
    [SerializeField] private BaseSlime_StickingState sticking;
    public Dictionary<PlayerStates, State> PlayerStatesDictionary = new Dictionary<PlayerStates, State>();

    private void Awake()
    {
        PlayerStatesDictionary.Add(PlayerStates.Idle, idle);
        PlayerStatesDictionary.Add(PlayerStates.Moving, moving);
        PlayerStatesDictionary.Add(PlayerStates.Airborne, airborne);
        PlayerStatesDictionary.Add(PlayerStates.Sticking, sticking);


        if (PlayerStatesDictionary.TryGetValue(PlayerStates.Idle, out State state))
        {
            currentState = state;
            currentState.EnterState();
        }
    }

    private void Update()
    {
        if (currentState.StateKey == currentState)
        {
            currentState.UpdateState();
        }
        else
        {
            currentState = currentState.StateKey;
            currentState.EnterState();
            UpdatePlayerState(currentState);
        }
    }

    private void UpdatePlayerState(State state)
    {
        switch (state)
        {
            case BaseSlime_IdleState:
                playerStates = PlayerStates.Idle;
                break;
            case BaseSlime_MovingState:
                playerStates = PlayerStates.Moving;
                break;
            case BaseSlime_AirborneState:
                playerStates = PlayerStates.Airborne;
                break;
            case BaseSlime_StickingState:
                playerStates = PlayerStates.Sticking;
                break;
        }
    }


}
