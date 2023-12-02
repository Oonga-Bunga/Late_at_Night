using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : AInteractable
{
    private bool _isOn = true; // Si el interruptor está o no encendido
    public event Action<bool> OnTurnedOnOrOff; // Evento que notifica al gamemanager de si este interruptor ha sido encendido o apagado
    public event Action<bool> OnAttacked;

    [Header("Switch Settings")]

    [SerializeField] private GameObject _emissiveObject;
    [SerializeField] private GameObject _nonEmissiveObject;
    [SerializeField] private int _hitsRequired = 5;
    private int _currentHits = 0;
    [SerializeField] private float _regenCooldown = 5;
    private float _currentTime = 5;
    private int _currentAttackingEnemies = 0;

    public bool IsOn => _isOn;

    protected override void Awake()
    {
        base.Awake();

        _emissiveObject.SetActive(true);
        _nonEmissiveObject.SetActive(false);
        _currentHits = 0;
        _currentTime = _regenCooldown;
        TurnOn();
    }

    protected override void Update()
    {
        base.Update();

        if (_currentAttackingEnemies == 0)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _regenCooldown)
            {
                _currentTime = Mathf.Max(0, _currentHits - 1);
                _currentTime = 0;
            }
        }
    }

    protected override void InteractedHoldAction()
    {
        TurnOn();
    }

    private void TurnOn()
    {
        _isOn = true;
        OnTurnedOnOrOff?.Invoke(_isOn);
        _canBeInteracted = false;
        DisableOutlineAndCanvas();
        _emissiveObject.SetActive(true);
        _nonEmissiveObject.SetActive(false);
    }

    private void TurnOff()
    {
        _isOn = false;
        OnTurnedOnOrOff?.Invoke(_isOn);
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

    public void StartAttack()
    {
        if (_currentAttackingEnemies == 0) OnAttacked?.Invoke(true);
        _currentAttackingEnemies++;
    }

    public void EndAttack()
    {
        _currentAttackingEnemies--;
        if (_currentAttackingEnemies == 0) OnAttacked?.Invoke(false);
    }
}
