using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : state
{
    //referencia al PlayerController
    public PlayerController playerController;
    //referencia a la linterna
    public Flashlight flashlight;
    public float vida = 1f;
    public Transform Objetivo;
    public float Velocidad;
    public NavMeshAgent IA;

    //estados del enemigo
    public ChaseState ChaseState;
    public Atackstate Atackstate;
    public float damageDistance = 100.0f; // Distancia para hacer daño
    public float damageAmount = 1.0f;  // Cantidad de daño
    public bool canseeplayer;
    public float detectionRadius = 10.0f; // Ajusta este valor al radio de detección

    public float tiempo = 2f;
    public float tiempoAtaque = 0f;

    public void seguirenemigo()
    {
        IA.speed = Velocidad;
        IA.SetDestination(Objetivo.position);
    }

    public override state RunCurrentState()
    {

        if(vida == 0){
            return ChaseState;
        }
        /*if (playerController.CurrentHeldObject.holdableObjectType != IPlayerReceiver.HoldableObjectType.Flashlight)
            {
                Debug.Log("usa");
                seguirenemigo();
                return ChaseState;
            }

            */
        else
        {
            tiempoAtaque += Time.deltaTime;
            // Buscar el objeto más cercano en la capa "Baby"
            Collider[] babyObjects = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Baby"));

            if (babyObjects.Length > 0)
            {
                // Seleccionar el objeto más cercano
                Transform closestBaby = babyObjects[0].transform;
                float closestDistance = Vector3.Distance(transform.position, closestBaby.position);

                for (int i = 1; i < babyObjects.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, babyObjects[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestBaby = babyObjects[i].transform;
                        closestDistance = distance;
                        
                    }
                }

                // Establecer el objeto más cercano como objetivo
                Objetivo = closestBaby;
                seguirenemigo();
                
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