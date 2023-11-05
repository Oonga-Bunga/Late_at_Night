using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AKillableEntity : MonoBehaviour, IKillableEntity
{
    #region Attributes

    [SerializeField] protected float _maxHealth;
    protected float _currentHealth;
    protected Collider _hitbox;
    public EventHandler<float> HealthChanged;

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor de la clase abstracta
    /// </summary>
    /// <param name="health">Vida máxima</param>
    public AKillableEntity(float health)
    {
        this._maxHealth = health;
        this._currentHealth = _maxHealth;
        this._hitbox = GetComponent<Collider>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Este método se invocaría cada vez que este gameobject recibe un golpe
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        ChangeHealth(damage, true);
    }

    /// <summary>
    /// Resta el valor de Value a la _currentHealth de la entidad si isDamage es true, y si es false lo suma.
    /// Si la entidad llega a 0 de vida muere.
    /// </summary>
    /// <param name="value">Daño o vida recibido</param>
    /// <param name="isDamage">Si es daño o recuperación de vida</param>
    public virtual void ChangeHealth(float value, bool isDamage)
    {
        if (isDamage)
        {
            _currentHealth = Mathf.Max(_currentHealth - value, 0);
            HealthChanged?.Invoke(this, _currentHealth);
        }
        else
        {
            _currentHealth = Mathf.Min(_currentHealth + value, _maxHealth);
            HealthChanged?.Invoke(this, _currentHealth);
        }
    }

    /// <summary>
    /// Si _currentHealth <= 0 destruye el objeto
    /// </summary>
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    #endregion

}