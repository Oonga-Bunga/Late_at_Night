using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilBunny
{
    public abstract class EvilBunnyState : MonoBehaviour
    {
        [SerializeField] protected EvilBunnyStateManager statemanager;
        public abstract State RunCurrentState(AMonster enemy);

    }
}
