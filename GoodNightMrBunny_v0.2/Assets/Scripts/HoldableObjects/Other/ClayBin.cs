using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBin : AHoldableObject
{
    void Start()
    {
        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBin;
    }
}
