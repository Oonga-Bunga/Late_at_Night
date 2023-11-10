using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBinPickup : AInteractable
{
    protected override void InteractedPressAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBalls, true, ClayBalls.Instance.MaxBallNumber);
    }

    protected override void InteractedHoldAction()
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.ClayBin, true);
        Destroy(gameObject);
    }
}
