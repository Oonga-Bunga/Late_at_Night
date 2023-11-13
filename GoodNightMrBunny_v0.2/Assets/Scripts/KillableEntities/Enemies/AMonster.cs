using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public abstract class AMonster : AKillableEntity
{
    #region Attributes

    [SerializeField] protected float _damage = 2f;
    [SerializeField] protected float _speed = 5f;
    protected Animator _animator;
    protected Rigidbody _rb;

    public float Damage => _damage;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor de la clase abstracta
    /// </summary>
    /// <param name="health">Vida máxima</param>
    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    #endregion

    #region Methods


    #endregion

}