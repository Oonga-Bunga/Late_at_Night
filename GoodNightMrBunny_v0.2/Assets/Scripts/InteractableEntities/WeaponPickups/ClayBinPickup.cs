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
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBalls, true, ClayBalls.maxBallNumber);
    }

    protected override void InteractedHoldAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBin, true);
        Destroy(gameObject);
    }
}
