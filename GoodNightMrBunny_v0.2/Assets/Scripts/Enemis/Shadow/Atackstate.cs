using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atackstate : State
{
    public override State RunCurrentState(AMonster enemy)
   {
    Debug.Log("atacado");
    return this;
   }
}
