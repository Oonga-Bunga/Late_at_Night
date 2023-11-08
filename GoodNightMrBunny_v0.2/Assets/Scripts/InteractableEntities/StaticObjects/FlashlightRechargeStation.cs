using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightRechargeStation : AInteractable
{
    [Header("Flashlight Recharge Station Settings")]

    [SerializeField, Range(0.0f, 1.0f)] private float _rechargeRate; // Velocidad de recarga
    private bool _hasFlashlight = true; // Si tiene una linterna cargándose o no
    private float _currentCharge = 0f; // Carga actual de la linterna que posee
    private float _rechargeAmount; // Cantidad de energia que se carga la linterna por segundo, depende de _rechargeRate y el maxCharge de la linterna
    [SerializeField] private GameObject _flashlightModel;
    private bool _isBeingAttacked = false;

    public bool IsBeingAttacked
    {
        get { return _isBeingAttacked; }
    }

    private void Start()
    {
        _flashlightModel.SetActive(_hasFlashlight);
        _rechargeAmount = Flashlight.maxCharge * _rechargeRate;
    }

    /// <summary>
    /// Si tiene una linterna que no está cargada del todo le va sumando carga con el tiempo
    /// </summary>
    private void Update()
    {
        if (_hasFlashlight && _currentCharge != Flashlight.maxCharge)
        {
            _currentCharge = Mathf.Min(_currentCharge + _rechargeAmount * Time.deltaTime, Flashlight.maxCharge);
        }
    }

    /// <summary>
    /// Si la estación de recarga tiene una linterna se la da al jugador, que dropea lo que tuviese equipado, 
    /// en caso contrario si el jugador tiene una linterna esta pasa a estar en la estación de recarga
    /// </summary>
    protected override void InteractedPressAction()
    {
        if (_player.CurrentHeldObject._holdableObjectType == IPlayerReceiver.HoldableObjectType.Flashlight)
        {
            if (_hasFlashlight)
            {
                _currentCharge = ((Flashlight)_player.CurrentHeldObject).CurrentCharge;
                _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, false, _currentCharge);
            }
            else
            {
                _currentCharge = ((Flashlight)_player.CurrentHeldObject).CurrentCharge;
                _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None, false);
                _hasFlashlight = true;
                _flashlightModel.SetActive(true);
            }
        }
        else if (_hasFlashlight)
        {
            _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.Flashlight, true, _currentCharge);
            _currentCharge = 0f;
            _hasFlashlight = false;
            _flashlightModel.SetActive(false);
        }
    }
}
