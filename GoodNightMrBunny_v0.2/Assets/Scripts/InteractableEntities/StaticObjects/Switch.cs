using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class Switch : AInteractable
{
    private bool isOn; // Si el interruptor está o no encendido
    public EventHandler<bool> TurnedOnOrOff; // Evento que notifica

    private void Start()
    {
        interactType = IInteractable.InteractType.Hold;
        isOn = false;
    }

    protected override void InteractedHoldAction()
    {
        TurnOn();
    }

    protected void TurnOn()
    {
        isOn = true;
        TurnedOnOrOff.Invoke(this, isOn);
        canBeInteracted = false;
    }

    protected void TurnOff()
    {
        isOn = false;
        TurnedOnOrOff.Invoke(this, isOn);
        canBeInteracted = true;
    }
}
