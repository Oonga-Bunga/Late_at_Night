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
            currentHealth -= Mathf.RoundToInt(damage);
            Debug.Log("Enemy Hit! Current Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}