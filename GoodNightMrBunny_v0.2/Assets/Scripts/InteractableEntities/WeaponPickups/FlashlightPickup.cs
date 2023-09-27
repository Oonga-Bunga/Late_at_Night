using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : AInteractable
{
    private void Start()
    {
        interactType = IInteractable.InteractType.Press;
    }

    public override void InteractedPressAction()
    {
        player.ChangeEquippedObject(IPlayerReceiver.EquippableObjectType.Flashlight);
    }
}
