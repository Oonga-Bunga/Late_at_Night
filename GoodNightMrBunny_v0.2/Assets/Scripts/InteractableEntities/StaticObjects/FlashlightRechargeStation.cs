using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightRechargeStation : AInteractable
{
    private bool hasFlashlight;
    private float currentCharge;
    [SerializeField][Range(0.0f, 1.0f)] private float rechargeRate;
    private float rechargeAmount;

    private void Start()
    {
        interactType = IInteractable.InteractType.Press;
        hasFlashlight = false;
        currentCharge = 0f;
        rechargeAmount = Flashlight.maxCharge * rechargeRate;
    }

    private void Update()
    {
        if (hasFlashlight && currentCharge != Flashlight.maxCharge)
        {
            currentCharge = Mathf.Min(currentCharge + rechargeAmount * Time.deltaTime, Flashlight.maxCharge);
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
            currentCharge = ((Flashlight)player.CurrentHeldObject).CurrentCharge;
            player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None, false);
            hasFlashlight = true;
        }
    }
}