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

    public Transform Objetivo;
    public float Velocidad;
    public NavMeshAgent IA;

    //estados del enemigo
    public ChaseState ChaseState;
    public Atackstate Atackstate;

    public bool canseeplayer;
    public float detectionRadius = 10.0f; // Ajusta este valor al radio de detección

    public void seguirenemigo()
    {
        IA.speed = Velocidad;
        IA.SetDestination(Objetivo.position);
    }

    public override state RunCurrentState()
    {
        if (playerController.CurrentHeldObject != playerController.GetComponentInChildren<EmptyWeapon>())
            {
                Debug.Log("usa");
                seguirenemigo();
                return ChaseState;
            }

            
        else
        {
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
                return this;
            }

            return this;
        }
    }

    void Update()
    {
        if (canseeplayer == true)
        {
            Debug.Log("0 vida");
        }
        else
        {
            // Debug.Log("Vida: " + vida);
        }
    }
}