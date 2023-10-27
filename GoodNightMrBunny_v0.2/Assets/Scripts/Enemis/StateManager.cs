using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private state currentState;
    [SerializeField] private AMonster enemy;
    void Update()
    {
        RunStateMachine();
    }
    private void RunStateMachine()
    {
        state nextState = currentState?.RunCurrentState(enemy);
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
