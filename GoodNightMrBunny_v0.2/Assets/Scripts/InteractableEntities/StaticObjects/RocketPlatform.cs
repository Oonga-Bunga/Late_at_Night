using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPlatform : AInteractable
{
    private bool isRocketReady;
    private bool isPlayerMounted;
    private float rocketCooldown;

    private void Start()
    {
        interactType = IInteractable.InteractType.Press;
    }

    protected override void InteractedPressAction()
    {

    }
}
