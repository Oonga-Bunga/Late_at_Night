using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : state
{
      public override state RunCurrentState(AMonster enemy)
   {
    Debug.Log("atacado");
    return this;
   }

}