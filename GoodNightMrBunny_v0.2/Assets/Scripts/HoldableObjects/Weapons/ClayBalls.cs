using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AHoldableObject
{
    #region Attributes

    private static ClayBalls _instance;

    public static ClayBalls Instance => _instance;

    private int _currentBallNumber;
    [SerializeField] private int _maxBallNumber = 6;
    [SerializeField] private float _baseDamage = 5f;
    [SerializeField] private UpdateUIClayAmmo _uiClayAmmo;
    [SerializeField] private GameObject _clayBallPrefab;
    [SerializeField] private float _shotForce = 20f;
    
    public int MaxBallNumber => _maxBallNumber;

    #endregion

    #region Methods

    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = this;
        }

        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
        _clayBallPrefab.GetComponent<ClayBallBehaviour>().baseDamage = _baseDamage;

        gameObject.SetActive(false);
    }

    public override void Initialize(float ballNumber)
    {
        //_uiClayAmmo.gameObject.SetActive(true);
        _currentBallNumber = Mathf.Min((int)ballNumber, _maxBallNumber);
        //_uiClayAmmo.setMaxBallNumber(_maxBallNumber);
        //_uiClayAmmo.UpdateClayText(_currentBallNumber);
    }

    /// <summary>
    /// Llama a la acción de disparar con el input del jugador
    /// </summary>
    /// <param name="attackInput">Input del jugador</param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (_currentBallNumber < 0) return;

        if (attackInput == IPlayerReceiver.InputType.Up)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Método que dispara bolas de plastilina
    /// </summary>
    public void Shoot()
    {
        if (_currentBallNumber <= 0) return;

        GameObject clayBall = Instantiate(_clayBallPrefab, this.transform.position, Quaternion.identity);
        clayBall.GetComponent<ClayBallBehaviour>().Initialize(transform.forward, _shotForce);

        _currentBallNumber -= 1;
        if (_currentBallNumber == 0)
        {
            //_uiClayAmmo.gameObject.SetActive(false);
            _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None,false);
            gameObject.SetActive(false);
            return;
        }

        //_uiClayAmmo.UpdateClayText(_currentBallNumber);
    }
    
    #endregion
}
