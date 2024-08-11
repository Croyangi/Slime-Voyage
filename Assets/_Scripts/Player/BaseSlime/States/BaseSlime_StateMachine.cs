using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlime_StateMachine : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle, Moving, Jumping, Airborne, Sticking, Compressed, LookingUp, Emote, OnEdge
    }

    [SerializeField] public PlayerStates playerStates;
    [SerializeField] public State currentState;
    [SerializeField] private BaseSlime_IdleState idle;
    [SerializeField] private BaseSlime_MovingState moving;
    [SerializeField] private BaseSlime_AirborneState airborne;
    [SerializeField] private BaseSlime_StickingState sticking;
    [SerializeField] private BaseSlime_CompressedState compressed;
    [SerializeField] private BaseSlime_LookingUp lookingUp;
    [SerializeField] private BaseSlime_Emote emote;
    [SerializeField] private BaseSlime_OnEdge onEdge;
    public Dictionary<PlayerStates, State> PlayerStatesDictionary = new Dictionary<PlayerStates, State>();

    private void Awake()
    {
        PlayerStatesDictionary.Add(PlayerStates.Idle, idle);
        PlayerStatesDictionary.Add(PlayerStates.Moving, moving);
        PlayerStatesDictionary.Add(PlayerStates.Airborne, airborne);
        PlayerStatesDictionary.Add(PlayerStates.Sticking, sticking);
        PlayerStatesDictionary.Add(PlayerStates.Compressed, compressed);
        PlayerStatesDictionary.Add(PlayerStates.LookingUp, lookingUp);
        PlayerStatesDictionary.Add(PlayerStates.Emote, emote);
        PlayerStatesDictionary.Add(PlayerStates.OnEdge, onEdge);
    }

    private void Start()
    {
        if (PlayerStatesDictionary.TryGetValue(PlayerStates.Idle, out State state))
        {
            currentState = state;
            currentState.EnterState();
        }
    }

    private void FixedUpdate() // Physics updates
    {
        if (currentState.StateKey == currentState)
        {
            currentState.FixedUpdateState();
        }
        else
        {
            currentState = currentState.StateKey;
            currentState.EnterState();
            UpdatePlayerState(currentState);
        }
    }

    private void Update() // Non-Physics update
    {
        if (currentState.StateKey == currentState)
        {
            currentState.UpdateState();
        }
    }

    private void UpdatePlayerState(State state)
    {
        //Debug.Log(state);

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
            case BaseSlime_CompressedState:
                playerStates = PlayerStates.Compressed;
                break;
            case BaseSlime_LookingUp:
                playerStates = PlayerStates.LookingUp;
                break;
            case BaseSlime_Emote:
                playerStates = PlayerStates.Emote;
                break;
        }
    }


}
