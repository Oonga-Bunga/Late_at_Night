using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace EvilBunny
{
    public class Spawn : State
    {
        [SerializeField] private State chaseState;
        // Start is called before the first frame update
        public override State RunCurrentState(AMonster enemy)
        {
            GameObject babyObjects = GameObject.FindGameObjectsWithTag("Baby")[0];
            if ( babyObjects != null)
            {
                ((EvilBunnyStateManager)stateManager).Baby = babyObjects;
                return chaseState;
            }
            ((EvilBunny)stateManager.Enemy).Objective = transform.position;

            return this;
        }   
    }
}
