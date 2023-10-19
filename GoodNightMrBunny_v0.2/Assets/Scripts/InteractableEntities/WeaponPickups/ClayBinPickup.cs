using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBinPickup : AInteractable
{
    private void Start()
    {

    }

    protected override void InteractedPressAction()
    {
        player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBalls, true, ClayBalls.maxBallNumber);
    }

    protected override void InteractedHoldAction()
    {
        player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBin, true);
        Destroy(gameObject);
    }
}
