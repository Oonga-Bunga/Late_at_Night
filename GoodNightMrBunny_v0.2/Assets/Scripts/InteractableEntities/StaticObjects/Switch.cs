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
    [SerializeField] private float _hitsRequired = 20f;
    private float _currentHits = 20f;

    public bool IsOn => _isOn;

    protected override void Awake()
    {
        base.Awake();

        _light.enabled = false;
        _currentHits = _hitsRequired;
    }

    protected override void Update()
    {
        base.Update();

        if (_currentHits <= 0)
        {
            TurnOff();
            _currentHits = _hitsRequired;
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
        _currentHits = Mathf.Max(0, _currentHits - 1);
    }
}
