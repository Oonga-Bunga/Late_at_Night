using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class Switch : AInteractable
{
    private bool _isOn = false; // Si el interruptor está o no encendido
    public EventHandler<bool> OnTurnedOnOrOff; // Evento que notifica al gamemanager de si este interruptor ha sido encendido o apagado
    [SerializeField] private Light _light;

    private void Start()
    {
        _light.enabled = false;
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
        _light.enabled = true;
    }

    private void TurnOff()
    {
        _isOn = false;
        OnTurnedOnOrOff.Invoke(this, _isOn);
        _canBeInteracted = true;
        _light.enabled = false;
    }
}
