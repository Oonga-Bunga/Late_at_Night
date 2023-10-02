using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightRechargeStation : AInteractable
{
    private bool hasFlashlight;
    private float currentCharge;
    [SerializeField] private float rechargeRate;

    private void Start()
    {
        interactType = IInteractable.InteractType.Press;
        hasFlashlight = false;
    }

    private void Update()
    {
        if (hasFlashlight && currentCharge != Flashlight.maxCharge)
        {
            currentCharge = Mathf.Min(currentCharge + rechargeRate * Time.deltaTime, Flashlight.maxCharge);
        }
    }

    protected override void InteractedPressAction()
    {
        if (hasFlashlight)
        {
            player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, currentCharge);
            hasFlashlight = false;
        }
        else if (player.CurrentHeldObject.holdableObjectType == IPlayerReceiver.HoldableObjectType.Flashlight)
        {
            player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None, false);
            hasFlashlight = true;
        }
    }
}
