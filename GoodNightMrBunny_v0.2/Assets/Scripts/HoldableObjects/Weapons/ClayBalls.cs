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
    [SerializeField] private GameObject _clayBallPrefab;
    [SerializeField] private float _shotForce = 30f;
    [SerializeField] private GameObject[] _handClayBalls;
    
    public float BaseDamage => _baseDamage;

    public int MaxBallNumber => _maxBallNumber;

    #endregion

    #region Methods

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;

        gameObject.SetActive(false);
    }

    public override void Initialize(float ballNumber)
    {
        _currentBallNumber = Mathf.Min((int)ballNumber, _maxBallNumber);
        foreach (var ball in _handClayBalls)
        {
            ball.gameObject.SetActive(true);
        }
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

        Quaternion rotacionDisparo = transform.rotation;
        rotacionDisparo *= Quaternion.Euler(-20f, 0f, 0f);
        GameObject clayBall = Instantiate(_clayBallPrefab, this.transform.position, rotacionDisparo);
        
        clayBall.GetComponent<ClayBallBehaviour>().Initialize(clayBall.transform.forward, _shotForce);

        _currentBallNumber -= 1;
        if (_currentBallNumber == 0)
        {
            _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None,false);
            gameObject.SetActive(false);
            return;
        }

        _handClayBalls[_currentBallNumber-1].gameObject.SetActive(false);
    }
    
    #endregion
}
