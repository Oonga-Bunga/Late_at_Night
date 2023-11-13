using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AMonster : AKillableEntity
{
    #region Attributes

    [SerializeField] protected float _damage = 2f;
    [SerializeField] protected float _speed = 5f;
    [SerializeField] protected Animator _animator;

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
    }

    #endregion

    #region Methods


    #endregion

}