using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashlightPickup : AInteractable
{
    private float _currentCharge = 100f;

    protected override void InteractedPressAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, _currentCharge);
        Destroy(gameObject);
    }

    public override void Initialize(float charge)
    {
        _currentCharge = charge;
    }
}