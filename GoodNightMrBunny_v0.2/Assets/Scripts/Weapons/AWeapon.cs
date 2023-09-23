using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeapon : MonoBehaviour, IWeapon
{
    public IPlayerReceiver.WeaponType weaponType;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected GameObject weaponPickupPrefab;

    public virtual void Attack(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop()
    {
        if (weaponPickupPrefab != null)
        {
            Instantiate(weaponPickupPrefab);
        }
        
        gameObject.SetActive(false);
    }
}
