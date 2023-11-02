using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    //referencia a la linterna
    [SerializeField] private Transform Objetivo;
    [SerializeField] private float Velocidad;
    [SerializeField] private NavMeshAgent IA;
    //estados del enemigo
    [SerializeField] private ChaseState ChaseState;
    [SerializeField] private Atackstate Atackstate;
    [SerializeField] private float damageDistance = 100.0f; // Distancia para hacer daño
    [SerializeField] private float damageAmount = 1.0f;  // Cantidad de daño
    public bool canseeplayer;
    [SerializeField] private float detectionRadius = 10.0f; // Ajusta este valor al radio de detección

    [SerializeField] private float tiempo = 2f;
    [SerializeField] private float tiempoAtaque = 0f;

    public void Seguirenemigo()
    {
        IA.speed = Velocidad;
        IA.SetDestination(Objetivo.position);
    }

    public override State RunCurrentState(AMonster enemy)
    {

        if(enemy.CurrentHealth == 0){
            return ChaseState;
        }
        
        else
        {
            tiempoAtaque += Time.deltaTime;
            // Buscar el objeto más cercano en la capa "Baby"            
            GameObject babyObjects = GameObject.FindGameObjectsWithTag("Baby")[0];
            if (Vector3.Distance(transform.position, babyObjects.transform.position) <= detectionRadius)
            {
                // Seleccionar el objeto más cercano
                Transform closestBaby = babyObjects.transform;                                            
                // Establecer el objeto más cercano como objetivo
                Objetivo = closestBaby;
                Seguirenemigo();
                
                // Verificar si el objetivo está lo suficientemente cerca para hacer daño
                float distanceToTarget = Vector3.Distance(transform.position, Objetivo.position);
                
                if (distanceToTarget <= damageDistance && tiempoAtaque >=tiempo)
                {
                    // Llamar a la función TakeHit del objetivo para hacer daño
                    if (Objetivo.TryGetComponent<AKillableEntity>(out var killableEntity))
                    {
                        tiempoAtaque = 0;
                        killableEntity.TakeHit(damageAmount);
                        
                    }
                }
                return this;
            }
            return this;
        }
    }

    void Update()
    {

       
    }
}