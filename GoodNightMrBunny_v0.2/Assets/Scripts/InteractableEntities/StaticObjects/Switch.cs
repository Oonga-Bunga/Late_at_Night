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
    private bool isBeingAttacked = false;
    [SerializeField] private float health = 20f;

    public bool IsBeingAttacked
    {
        get { return isBeingAttacked; }
        set { isBeingAttacked = value; }
    }

    public void TakeHit()
    {
        health = Mathf.Max(0, health - 1);
    }

    public bool IsOn
    {
        get { return _isOn; }
    }

    private void Start()
    {
        _light.enabled = false;
    }

    private void Update()
    {
        if (health <= 0)
        {
            TurnOff();
        }
    }

    protected override void InteractedHoldAction()
    {
        TurnOn();
    }

    private void TurnOn()
    {
        _isOn = true;
        OnTurnedOnOrOff?.Invoke(this, _isOn);
        _canBeInteracted = false;
        _light.enabled = true;
    }

    private void TurnOff()
    {
        _isOn = false;
        OnTurnedOnOrOff?.Invoke(this, _isOn);
        _canBeInteracted = true;
        _light.enabled = false;
    }
}
