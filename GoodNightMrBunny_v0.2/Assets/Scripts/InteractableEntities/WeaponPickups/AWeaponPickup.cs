using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeaponPickup : AInteractable
{
    public IPlayerReceiver.PickupType weaponType;

    public override void Interacted(object player)
    {
        ((IPlayerReceiver)player).ChangeEquippedObject(weaponType);
        Destroy(gameObject);
    }
}
