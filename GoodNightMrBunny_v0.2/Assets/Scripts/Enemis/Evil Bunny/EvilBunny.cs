using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace EvilBunny
{
    public class EvilBunny : AMonster
    {
        [SerializeField] private float Velocidad;
        [SerializeField] private NavMeshAgent IA;
        [SerializeField] private Transform Objetivo;

        public override void TakeHit(float damage, GameManager.AttackSource source)
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
        protected override void Die()
        {
            Destroy(transform.parent.gameObject);
            //Destroy(this);

            Debug.Log("morision");
            //gameObject.Destroy(this);
        }
        public void Seguirenemigo()
        {
            IA.speed = Velocidad;
            IA.SetDestination(Objetivo.position);
        }

    }
}

    
