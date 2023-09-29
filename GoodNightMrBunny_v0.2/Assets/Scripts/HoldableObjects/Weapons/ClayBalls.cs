using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AWeapon
{
    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
    }
}
