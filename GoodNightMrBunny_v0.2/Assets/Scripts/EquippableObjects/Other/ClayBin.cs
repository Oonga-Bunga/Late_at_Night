using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBin :AEquippableObject
{
    void Start()
    {
        pickupType = IPlayerReceiver.EquippableObjectType.ClayBin;
    }
}
