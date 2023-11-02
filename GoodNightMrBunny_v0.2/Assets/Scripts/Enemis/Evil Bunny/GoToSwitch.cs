using EvilBunny;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilBunny
{
    public class GoToSwitch : State
    {
        [SerializeField] private State chaseState;
        [SerializeField] private State turningOffSwitch;
        [SerializeField] private float attackRadius = 1f;

        public override State RunCurrentState(AMonster enemy)
        {
            if (!((EvilBunnyStateManager)stateManager).SwitchGoal.GetComponent<Switch>().IsOn)
            {
                return chaseState;
            }

            Vector3 switchPos = ((EvilBunnyStateManager)stateManager).SwitchGoal.transform.position;
            float distanceToTarget = Vector3.Distance(transform.position, switchPos);
            if (distanceToTarget <= attackRadius)
            {
                return turningOffSwitch;
            }

            ((EvilBunny)stateManager.Enemy).Objective = switchPos;
            return this;
        }
    }
}
