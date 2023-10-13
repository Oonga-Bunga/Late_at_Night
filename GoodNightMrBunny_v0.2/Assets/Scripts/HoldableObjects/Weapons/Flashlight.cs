using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : AWeapon
{
    private float currentCharge;
    static public float maxCharge = 100f;

    public float CurrentCharge
    {
        get { return currentCharge; }
    }

    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.Flashlight;
        currentCharge = maxCharge;
    }

    public override void Initialize(float charge)
    {
        currentCharge = charge;
    }
}
