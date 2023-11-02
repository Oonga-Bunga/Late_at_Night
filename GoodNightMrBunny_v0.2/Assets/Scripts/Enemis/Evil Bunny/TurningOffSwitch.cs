using EvilBunny;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningOffSwitch : State
{
    [SerializeField] private State chaseState;

    public override State RunCurrentState(AMonster enemy)
    {
        if (!((EvilBunnyStateManager)stateManager).SwitchGoal.GetComponent<Switch>().IsOn)
        {
            return chaseState;
        }

        //si la animaci�n no est� en curso activarla

        return this;
    }
}
