using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EvilBunny
{
    public class EvilBunny : AMonster
    {
        public override void TakeHit(float damage, GameManager.AttackSource source)
        {
            if (currentHealth > 0)
            {
                // Reducir la salud actual del enemigo en funci�n del da�o proporcionado.
                currentHealth -= Mathf.RoundToInt(damage);
                //Debug.Log("recibiendo da�o");
                if (currentHealth <= 0)
                {
                    Die();


                }
            }
        }
        protected override void Die()
        {
            Destroy(transform.parent.gameObject);
            //Destroy(this);

            Debug.Log("morision");
            //gameObject.Destroy(this);
        }
    }
}

    
