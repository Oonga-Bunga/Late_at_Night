using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : AHoldableObject
{
    void Start()
    {
        _holdableObjectType = IPlayerReceiver.HoldableObjectType.None;
    }
}
