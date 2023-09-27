using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWeapon : AEquippableObject
{
    [SerializeField] private float baseDamage = 2f;
    [SerializeField] public IPlayerReceiver.EquippableObjectType pickupType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
