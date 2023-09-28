using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class Switch : AInteractable
{
    private bool isOn;

    private void Start()
    {
        interactType = IInteractable.InteractType.Hold;
        isOn = false;
    }

    protected override void InteractedHoldAction()
    {
        isOn = true;
        canBeInteracted = false;
    }
}
