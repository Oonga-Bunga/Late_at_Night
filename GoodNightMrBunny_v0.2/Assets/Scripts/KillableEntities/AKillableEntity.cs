using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AKillableEntity : MonoBehaviour, IKillableEntity
{
    #region Attributes

    [Header("AKillableEntity Settings")]

    [SerializeField] protected float _maxHealth = 10f;
    protected float _currentHealth = 0f;
    protected Collider _hitbox;
    public event Action<float> OnHealthChanged;
    public event Action OnDied;
    public event Action OnDamaged;

    public float MaxHealth => _maxHealth;

    public float CurrentHealth => _currentHealth;

    #endregion

    #region Initialization

    protected virtual void Awake()
    {
        _currentHealth = _maxHealth;
        _hitbox = GetComponent<Collider>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Este método se invocaría cada vez que este gameobject recibe un golpe
    /// </summary>
    /// <param name="damage">Cantidad de daño</param>
    /// <param name="source">Fuente del daño</param>
    public virtual void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        ChangeHealth(damage, true);
        OnDamaged?.Invoke();
    }

    /// <summary>
    /// Este método se invocaría cada vez que este gameobject recupere vida de una fuente externa
    /// </summary>
    /// <param name="amount">Cantidad de vida recuperada</param>
    public virtual void RecoverHealth(float amount)
    {
        ChangeHealth(amount, false);
    }

    /// <summary>
    /// Resta el valor de Value a la _currentHealth de la entidad si isDamage es true, y si es false lo suma.
    /// Si la entidad llega a 0 de vida muere.
    /// </summary>
    /// <param name="value">Daño o vida recibido</param>
    /// <param name="isDamage">Si es daño o recuperación de vida</param>
    protected virtual void ChangeHealth(float value, bool isDamage)
    {
        if (isDamage)
        {
            _currentHealth = Mathf.Max(_currentHealth - value, 0);
            OnHealthChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            _currentHealth = Mathf.Min(_currentHealth + value, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }

    /// <summary>
    /// Destruye el objeto
    /// </summary>
    public virtual void Die()
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }

    #endregion

}