using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : AWeapon
{
    void Start()
    {
        pickupType = IPlayerReceiver.EquippableObjectType.Flashlight;
    }
}
