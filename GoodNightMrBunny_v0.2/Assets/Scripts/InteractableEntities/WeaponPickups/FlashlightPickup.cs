using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : AInteractable
{
    private float currentCharge = 100f;

    private void Start()
    {

    }

    protected override void InteractedPressAction()
    {
        player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, currentCharge);
        Destroy(gameObject);
    }

    public void Initialize(float charge)
    {
        currentCharge = charge;
    }
}
