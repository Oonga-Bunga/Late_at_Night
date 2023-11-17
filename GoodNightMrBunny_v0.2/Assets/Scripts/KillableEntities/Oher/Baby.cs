using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Baby : AKillableEntity
{
    private static Baby _instance;

    public static Baby Instance => _instance;

    [SerializeField] private int maxEvilBunnies;
    private int nEvilBunnies = 0;

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
        nEvilBunnies++;
        //Actualizar morpher
        if(nEvilBunnies >= maxEvilBunnies)
        {
            //activar animación de ataque del bicho gigante
            //cuando termine el jugador pierde
            Die();
        }
    }

    public override void Die()
    {
        Died?.Invoke(this, true);
    }
}
