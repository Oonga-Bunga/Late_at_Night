using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : state
{
    // Asegúrate de tener una referencia al PlayerController
    public PlayerController playerController;

    public Flashlight flashlight;
    public int vida;
    public Transform Objetivo;
    public float Velocidad;
    public NavMeshAgent IA;

    public ChaseState ChaseState;
    public Atackstate Atackstate;
    public bool canseeplayer;

    public override state RunCurrentState()
    {
        if (vida <= 0)
        {
            return ChaseState;
        }
        else
        {
            // Cambiar al estado de Chase si el jugador tiene un objeto
            if (playerController.CurrentHeldObject != playerController.GetComponentInChildren<EmptyWeapon>())
            {
                Debug.Log("usa");
                return ChaseState;
            }

            return this;
        }
    }

    void Update()
    {
        IA.speed = Velocidad;
        IA.SetDestination(Objetivo.position);

        if (vida <= 0)
        {
            Debug.Log("0 vida");
        }
        else
        {
            // Debug.Log("Vida: " + vida);
        }
    }
}