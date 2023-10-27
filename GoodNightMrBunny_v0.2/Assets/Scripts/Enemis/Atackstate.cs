using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atackstate : state
{
    public override state RunCurrentState(AMonster enemy)
   {
    Debug.Log("atacado");
    return this;
   }
}
