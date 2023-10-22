using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMonster : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeHit(float damage)
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

    private void Die()
    {
        Destroy(transform.parent.gameObject);
        //Destroy(this);
        
        Debug.Log("morision");
        //gameObject.Destroy(this);
    }
}