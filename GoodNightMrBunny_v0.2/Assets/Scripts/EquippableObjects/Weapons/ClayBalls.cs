using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AWeapon
{
    void Start()
    {
        pickupType = IPlayerReceiver.EquippableObjectType.ClayBalls;
    }
}
