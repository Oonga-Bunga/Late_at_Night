using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace EvilBunny
{
    public class Spawn : State
    {
        [SerializeField] private State ChaseState;
        // Start is called before the first frame update
        public override State RunCurrentState(AMonster enemy)
        {
            GameObject babyObjects = GameObject.FindGameObjectsWithTag("Baby")[0];
            if ( babyObjects != null)
            {
                statemanager.
                return ChaseState;
            }
            return this;

        }   
    }
}
