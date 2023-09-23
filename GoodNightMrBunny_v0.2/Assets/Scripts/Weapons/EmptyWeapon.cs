using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : AWeapon
{
    void Start()
    {
        weaponType = IPlayerReceiver.WeaponType.Empty;
    }
}
