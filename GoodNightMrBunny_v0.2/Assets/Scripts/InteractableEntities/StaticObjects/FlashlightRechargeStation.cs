using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightRechargeStation : AInteractable
{
    [Header("Flashlight Recharge Station Settings")]

    [SerializeField, Range(0.0f, 1.0f)] private float _rechargeRate = 0.1f; // Velocidad de recarga
    [SerializeField, Range(0.0f, 1.0f)] private float _drainRate = 0.1f; // Velocidad de recarga
    [SerializeField] private float _drainDuration = 10f; // Velocidad de recarga
    private bool _hasFlashlight = true; // Si tiene una linterna cargándose o no
    private float _currentCharge = 0f; // Carga actual de la linterna que posee
    private float _rechargeAmount; // Cantidad de energia que se carga la linterna por segundo, depende de _rechargeRate y el _maxCharge de la linterna
    private float _drainAmount; // Cantidad de energia que se carga la linterna por segundo, depende de _rechargeRate y el _maxCharge de la linterna
    [SerializeField] private GameObject _flashlightModel;
    private bool _isBeingAttacked = false;
    private bool _isTaken = false;
    private bool _isDrained = false;

    public bool IsTaken
    {
        get { return _isTaken; }
        set { _isTaken = value; }
    }

    public bool HasFlashlight => _hasFlashlight;

    public bool IsDrained => _isDrained;

    protected override void Start()
    {
        base.Start();
        _flashlightModel.SetActive(_hasFlashlight);
        _rechargeAmount = Flashlight.Instance.MaxCharge * _rechargeRate;
        _drainAmount = Flashlight.Instance.MaxCharge * _drainRate;
        _currentCharge = Flashlight.Instance.MaxCharge;
    }

    /// <summary>
    /// Si tiene una linterna que no está cargada del todo le va sumando carga con el tiempo
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (_isDrained) { return; }

        if (_isBeingAttacked)
        {
            _currentCharge = Mathf.Max(_currentCharge - _drainAmount * Time.deltaTime, 0);

            if (_currentCharge == 0f)
            {
                _isDrained = true;
                Invoke("RecoverFromDrained", _drainDuration);
            }
        }
        else
        {
            if (_hasFlashlight && _currentCharge != Flashlight.Instance.MaxCharge)
            {
                _currentCharge = Mathf.Min(_currentCharge + _rechargeAmount * Time.deltaTime, Flashlight.Instance.MaxCharge);
            }
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

    public void BegginTheSucc()
    {
        _isBeingAttacked = true;
        _canBeInteracted = false;
    }

    public void StopTheSucc()
    {
        _isBeingAttacked = false;

        if (!_isDrained)
        {
            _canBeInteracted = true;
        }
    }

    private void RecoverFromDrained()
    {
        _isDrained = false;
        _canBeInteracted = true;
    }
}
