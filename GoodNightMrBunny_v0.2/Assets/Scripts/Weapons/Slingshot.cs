using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : AWeapon
{
    void Start()
    {
        weaponType = IPlayerReceiver.WeaponType.Slingshot;
    }
}