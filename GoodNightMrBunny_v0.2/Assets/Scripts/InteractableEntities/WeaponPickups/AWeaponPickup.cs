using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeaponPickup : AInteractable
{
    public IPlayerReceiver.WeaponType weaponType;

    public override void Interacted(object player)
    {
        ((IPlayerReceiver)player).ChangeWeapon(weaponType);
        Destroy(gameObject);
    }
}
