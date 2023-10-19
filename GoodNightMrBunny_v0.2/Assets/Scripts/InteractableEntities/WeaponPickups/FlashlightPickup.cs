using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : AInteractable
{
    private float _currentCharge = 100f;

    private void Start()
    {

    }

    protected override void InteractedPressAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, _currentCharge);
        Destroy(gameObject);
    }

    public void Initialize(float charge)
    {
        _currentCharge = charge;
    }
}
