using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : AWeapon
{
    void Start()
    {
        pickupType = IPlayerReceiver.HoldableObjectType.None;
    }
}
