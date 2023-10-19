using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class Switch : AInteractable
{
    private bool isOn = false; // Si el interruptor está o no encendido
    public EventHandler<bool> TurnedOnOrOff; // Evento que notifica

    private void Start()
    {

    }

    protected override void InteractedHoldAction()
    {
        TurnOn();
    }

    private void TurnOn()
    {
        isOn = true;
        TurnedOnOrOff.Invoke(this, isOn);
        canBeInteracted = false;
    }

    private void TurnOff()
    {
        isOn = false;
        TurnedOnOrOff.Invoke(this, isOn);
        canBeInteracted = true;
    }
}
