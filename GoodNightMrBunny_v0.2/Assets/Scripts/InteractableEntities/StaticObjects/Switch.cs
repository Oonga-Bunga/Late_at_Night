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
    private bool _isBeingAttacked = false;
    [SerializeField] private float _maxHealth = 20f;
    private float _currenthealth = 20f;

    public bool IsBeingAttacked
    {
        get { return _isBeingAttacked; }
        set { _isBeingAttacked = value; }
    }

    public bool IsOn
    {
        get { return _isOn; }
    }

    private void Start()
    {
        _light.enabled = false;
        _currenthealth = _maxHealth;
    }

    protected override void Update()
    {
        base.Update();

        if (_currenthealth <= 0)
        {
            TurnOff();
            _isBeingAttacked = false;
            _currenthealth = _maxHealth;
        }
    }

    protected override void InteractedHoldAction()
    {
        if (_isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
        
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

    public void TakeHit()
    {
        _currenthealth = Mathf.Max(0, _currenthealth - 1);
    }
}
