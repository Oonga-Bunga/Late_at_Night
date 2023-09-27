using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBinPickup : AInteractable
{
    private void Start()
    {
        interactType = IInteractable.InteractType.PressAndHold;
    }

    public override void InteractedPressAction()
    {
        player.ChangeEquippedObject(IPlayerReceiver.EquippableObjectType.ClayBalls);
    }

    public override void InteractedHoldAction()
    {
        player.ChangeEquippedObject(IPlayerReceiver.EquippableObjectType.ClayBin);
        Destroy(gameObject);
    }
}
