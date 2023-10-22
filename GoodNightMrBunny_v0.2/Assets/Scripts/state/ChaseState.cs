using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : state
{
      public override state RunCurrentState()
   {
    Debug.Log("atacado");
    return this;
   }

}