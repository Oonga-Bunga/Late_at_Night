using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : AWeaponPickup
{
    void Start()
    {
        weaponType = IPlayerReceiver.WeaponType.Flashlight;
    }
}
