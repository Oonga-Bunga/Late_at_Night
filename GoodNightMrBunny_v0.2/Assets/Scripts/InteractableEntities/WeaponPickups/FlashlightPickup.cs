using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashlightPickup : AInteractable
{
    [System.Serializable]
    public class PickupEvent : UnityEvent { }

    public PickupEvent onPickup;

    private float _currentCharge = 100f;

    protected override void InteractedPressAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, _currentCharge);
        onPickup.Invoke(); // Dispara el evento al recoger la linterna
        Destroy(gameObject);
    }

    public void Initialize(float charge)
    {
        _currentCharge = charge;
    }
}