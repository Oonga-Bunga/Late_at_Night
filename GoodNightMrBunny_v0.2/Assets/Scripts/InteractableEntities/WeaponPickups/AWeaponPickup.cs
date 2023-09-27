using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeaponPickup : AInteractable
{
    public IPlayerReceiver.EquippableObjectType weaponType;

    public override void InteractedPressAction()
    {
        player.ChangeEquippedObject(weaponType);
        Destroy(gameObject);
    }
}
