using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : AWeapon
{
    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.None;
    }
}
