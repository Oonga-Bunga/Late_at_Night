using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atackstate : state
{
    public override state RunCurrentState()
   {
    Debug.Log("atacado");
    return this;
   }
}
