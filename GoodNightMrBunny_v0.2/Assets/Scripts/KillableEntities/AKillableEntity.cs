using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AKillableEntity : MonoBehaviour, IKillableEntity
{
    #region Attributes

    [SerializeField] protected float maxHealth;
    protected float currentHealth;
    protected bool dead;
    protected Collider hitbox;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor de la clase abstracta
    /// </summary>
    /// <param name="health">Vida máxima</param>
    public AKillableEntity(float health)
    {
        this.maxHealth = health;
        this.currentHealth = maxHealth;
        this.dead = false;
        this.hitbox = GetComponent<Collider>();
    }

    #endregion

    #region Methods

    public virtual void TakeHit(float damage)
    {
        ChangeHealth(damage, true);
    }

    /// <summary>
    /// Resta el valor de Value a la currentHealth de la entidad si isDamage es true, y si es false lo suma.
    /// Si la entidad llega a 0 de vida muere.
    /// </summary>
    /// <param name="value">Daño o vida recibido</param>
    /// <param name="isDamage">Si es daño o recuperación de vida</param>
    public virtual void ChangeHealth(float value, bool isDamage)
    {
        if (isDamage)
        {
            currentHealth = Mathf.Max(currentHealth - value, 0);
            if (currentHealth == 0)
            {
                Die();
            }
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth + value, maxHealth);
        }
    }

    /// <summary>
    /// Si currentHealth <= 0 destruye el objeto
    /// </summary>
    public virtual void Die()
    {
        dead = true;
        Destroy(gameObject);
    }

    #endregion

}