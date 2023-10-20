using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
  public state currentState;
    void Update()
    {
        RunStateMachine();
    }
    private void RunStateMachine()
    {
        state nextState = currentState?.RunCurrentState();
        if(nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }
    private void SwitchToTheNextState(state nextState)
    {
        currentState = nextState;
    }
}
