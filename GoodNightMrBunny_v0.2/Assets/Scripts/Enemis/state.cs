using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected StateManager statemanager;
    public abstract State RunCurrentState(AMonster enemy);
    
}
