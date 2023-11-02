using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] protected State currentState;
    [SerializeField] protected AMonster enemy;
    [SerializeField] protected Animator animatorController;

    public AMonster Enemy
    {
        get { return enemy; }
    }

    void Update()
    {
        RunStateMachine();
    }
    protected void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(enemy);
        if(nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }
    protected void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
