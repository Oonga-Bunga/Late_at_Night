using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : AWeapon
{
    private float currentCharge;
    [SerializeField] private float maxCharge;

    void Start()
    {
        pickupType = IPlayerReceiver.HoldableObjectType.Flashlight;
    }

    public override void Initialize(float charge)
    {
        currentCharge = Mathf.Min(charge, maxCharge);
    }
}