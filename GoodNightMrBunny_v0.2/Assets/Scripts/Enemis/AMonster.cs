using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class AMonster : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    protected int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeHit(float damage,GameManager.AttackSource source)
    {
        if (currentHealth > 0)
        {
            // Reducir la salud actual del enemigo en función del daño proporcionado.
            currentHealth -= Mathf.RoundToInt(damage);
            //Debug.Log("recibiendo daño");
            if (currentHealth <= 0)
            {
                Die();
                
                
            }
        }
    }

    protected virtual void Die()
    {
        Destroy(transform.parent.gameObject);
        //Destroy(this);
        
        Debug.Log("morision");
        //gameObject.Destroy(this);
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
    }
}