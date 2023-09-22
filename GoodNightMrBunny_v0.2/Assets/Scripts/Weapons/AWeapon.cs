using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeapon : MonoBehaviour, IWeapon
{
    protected WeaponPickup weaponPickup;

    public virtual void Attack(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop()
    {

    }
}
