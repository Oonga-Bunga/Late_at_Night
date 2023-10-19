using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class Switch : AInteractable
{
    private bool _isOn = false; // Si el interruptor está o no encendido
    public EventHandler<bool> OnTurnedOnOrOff; // Evento que notifica al gamemanager de si este interruptor ha sido encendido o apagado

    private void Start()
    {

    }

    protected override void InteractedHoldAction()
    {
        TurnOn();
    }

    private void TurnOn()
    {
        _isOn = true;
        OnTurnedOnOrOff.Invoke(this, _isOn);
        _canBeInteracted = false;
    }

    private void TurnOff()
    {
        _isOn = false;
        OnTurnedOnOrOff.Invoke(this, _isOn);
        _canBeInteracted = true;
    }
}
