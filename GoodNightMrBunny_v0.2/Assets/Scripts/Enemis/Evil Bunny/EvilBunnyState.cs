using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvilBunny
{
    public abstract class EvilBunnyState : State
    {
        [SerializeField] protected EvilBunnyStateManager statemanager;

    }
}
