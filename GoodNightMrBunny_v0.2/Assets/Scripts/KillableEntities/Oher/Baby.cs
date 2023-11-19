using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Baby : AKillableEntity
{
    private static Baby _instance;

    public static Baby Instance => _instance;

    [SerializeField] private int _maxEvilBunnies;
    private int _nEvilBunnies = 0;
    [SerializeField] private Animator _amalgamateAnimator;

    [SerializeField] private Transform _zanybellPoint;
    [SerializeField] private Transform _evilBunnyPoint;

    public Transform ZanybellPoint => _zanybellPoint;
    public Transform EvilBunnyPoint => _evilBunnyPoint;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected void Update()
    {
        if (_amalgamateAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Obtener el estado actual de la animaci�n
            AnimatorStateInfo stateInfo = _amalgamateAnimator.GetCurrentAnimatorStateInfo(0);

            // Verificar si la animaci�n ha llegado al final
            if (stateInfo.normalizedTime >= 1.0f)
            {
                // La animaci�n ha llegado al final
                Die();
            }
        }
    }

    public override void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        base.TakeHit(damage, source);

        if (_currentHealth == 0) 
        {
            Die();
        }
    }

    public void EvilBunnyGoesUnderBed()
    {
        _nEvilBunnies++;
        _amalgamateAnimator.SetTrigger("Grow");
    }

    public override void Die()
    {
        Died?.Invoke(this, true);
    }
}
