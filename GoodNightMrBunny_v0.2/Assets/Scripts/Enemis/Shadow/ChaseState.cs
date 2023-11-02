using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
      public override State RunCurrentState(AMonster enemy)
   {
    Debug.Log("atacado");
    return this;
   }

}