using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : AInteractable
{
    private bool _isOn = true; // Si el interruptor está o no encendido
    public EventHandler<bool> OnTurnedOnOrOff; // Evento que notifica al gamemanager de si este interruptor ha sido encendido o apagado
    [SerializeField] private GameObject _emissiveObject;
    [SerializeField] private GameObject _nonEmissiveObject;
    [SerializeField] private float _hitsRequired = 20f;
    private float _currentHits = 0f;

    public bool IsOn => _isOn;

    protected override void Awake()
    {
        base.Awake();

        _emissiveObject.SetActive(true);
        _nonEmissiveObject.SetActive(false);
        _currentHits = 0;
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
        _emissiveObject.SetActive(true);
        _nonEmissiveObject.SetActive(false);
    }

    private void TurnOff()
    {
        _isOn = false;
        OnTurnedOnOrOff?.Invoke(this, _isOn);
        _canBeInteracted = true;
        _emissiveObject.SetActive(false);
        _nonEmissiveObject.SetActive(true);
    }

    public void TakeHit()
    {
        _currentHits = Mathf.Min(_hitsRequired, _currentHits + 1);

        if (_currentHits == _hitsRequired)
        {
            TurnOff();
            _currentHits = 0;
        }
    }
}
