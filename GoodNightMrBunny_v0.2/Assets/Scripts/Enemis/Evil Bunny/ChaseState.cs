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
        [SerializeField] private float 

        public override State RunCurrentState(AMonster enemy)
        {
            time += Time.deltaTime;

            if (time >= timeswitch)
            {
                GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
                foreach (GameObject closeSwitch in switches)
                {
                    if (closeSwitch.gameObject.GetComponent<Switch>().IsBeingAttacked)
                    {

                    }
                }
            }

        }
    }
}
