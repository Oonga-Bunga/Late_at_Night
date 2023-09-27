using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotPickup : AWeaponPickup
{
    void Start()
    {
        weaponType = IPlayerReceiver.EquippableObjectType.ClayBalls;
    }
}
