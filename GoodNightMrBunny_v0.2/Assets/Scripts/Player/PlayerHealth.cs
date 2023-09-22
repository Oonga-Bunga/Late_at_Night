using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHealth : AKillableEntity
{
    public EventHandler<float> HealthChanged;

    public PlayerHealth(float health) : base(health)
    {
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ChangePlayerHealth(float value, bool IsDamage)
    {
        ChangeHealth(value, IsDamage);
        HealthChanged?.Invoke(this, currentHealth);
    }

    public void PlayerDie()
    {
        Die();
    }
}
