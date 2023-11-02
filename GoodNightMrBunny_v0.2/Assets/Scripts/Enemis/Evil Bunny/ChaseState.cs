using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class ChaseState : State
    {
        [SerializeField] private float timeswitch = 10f;
        private float time = 0f;
        private bool hasTurnedOffSwitch = false;
        [SerializeField] private float detectionRadius = 10f;

        public override State RunCurrentState(AMonster enemy)
        {
            time += Time.deltaTime;

            if (time >= timeswitch)
            {
                GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
                GameObject bestSwitch = null;

                foreach (GameObject closeSwitch in switches)
                {
                    if (hasTurnedOffSwitch)
                    {
                        if (closeSwitch.gameObject.GetComponent<Switch>().IsBeingAttacked)
                        {
                            if ()
                        }
                    }
                    else
                    {

                    }
                }
            }

        }
    }
}
