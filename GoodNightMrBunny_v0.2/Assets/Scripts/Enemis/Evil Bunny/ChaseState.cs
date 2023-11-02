using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class ChaseState : State
    {
        [SerializeField] private float timeswitch = 10f;
        private float time = 0f;
        public override State RunCurrentState(AMonster enemy)
        {
            GameObject babyObjects = GameObject.FindGameObjectsWithTag("Baby")[0];
            if (babyObjects != null)
            {
                return ChaseState;
            }
            return this;

        }
    }
}
