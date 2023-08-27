using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerStates currentState { get; private set; }
    public void Initialize(PlayerStates playerStates)
    {
        currentState = playerStates;
        currentState.Enter();
    }

    public void ChangeState(PlayerStates playerStates)
    {
        currentState.Exit();
        currentState = playerStates;
        currentState.Enter();
    }
}
