using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class ChaseState : EvilBunnyState
    {
        [SerializeField] private State GoUnderBed;
        [SerializeField] private State GoToSwitch;

        [SerializeField] private float timeswitch = 10f;
        private float time = 0f;
        private bool hasTurnedOffSwitch = false;
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float attackRadius = 1f;

        public override State RunCurrentState(AMonster enemy)
        {
            float distanceToTarget = Vector3.Distance(transform.position, statemanager.Baby.transform.position);
            if (distanceToTarget <= attackRadius)
            {
                return GoUnderBed;
            }

            time += Time.deltaTime;

            if (time >= timeswitch)
            {
                GameObject[] switches = GameObject.FindGameObjectsWithTag("Switch");
                GameObject bestSwitch = null;
                float bestdistance = 10000f;

                foreach (GameObject closeSwitch in switches)
                {
                    if (hasTurnedOffSwitch)
                    {
                        if (closeSwitch.gameObject.GetComponent<Switch>().IsBeingAttacked)
                        {
                            float distance = Vector3.Distance(transform.position, closeSwitch.transform.position);
                            if (distance <= detectionRadius && distance < distanceToTarget){
                                if (distance < bestdistance)
                                {
                                    bestSwitch = closeSwitch;
                                    bestdistance = distance;
                                }
                            }
                        }
                    }
                    else
                    {
                        float distance = Vector3.Distance(transform.position, closeSwitch.transform.position);
                        if (distance <= detectionRadius && distance < distanceToTarget)
                        {
                            if (distance < bestdistance)
                            {
                                bestSwitch = closeSwitch;
                                bestdistance = distance;
                            }
                        }
                    }
                }
                if (bestSwitch != null)
                {
                    return GoToSwitch;
                }
            }           
        }
    }
}
