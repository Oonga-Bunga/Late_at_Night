using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : AWeapon
{
    void Start()
    {
        pickupType = IPlayerReceiver.EquippableObjectType.ClayBalls;
    }
}
