using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AMonster : AKillableEntity
{
    #region Attributes

    [Header("AMonster Settings")]

    [SerializeField] protected float _damage = 2f;
    [SerializeField] protected float _walkingSpeed = 5f;
    protected float _currentSpeed = 0f;
    [SerializeField] protected Animator _animator;
    protected Rigidbody _rb;
    protected PlayerController _player;

    public float Damage => _damage;

    #endregion

    #region Initialization

    /// <summary>
    /// Constructor de la clase abstracta
    /// </summary>
    /// <param name="health">Vida máxima</param>
    protected override void Awake()
    {
        base.Awake();

        _currentSpeed = _walkingSpeed;
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        _player = PlayerController.Instance;
    }

    #endregion
}