using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHealth : AKillableEntity
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void ChangeHealth(float value, bool IsDamage)
    {
        base.ChangeHealth(value, IsDamage);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        GameManager.Instance.PlayerLost();
    }
}
