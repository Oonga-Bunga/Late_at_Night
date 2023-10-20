using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : state
{
    public int vida;
    public int damage;

    public Transform Objetivo;
    public float Velocidad;
    public NavMeshAgent IA;

    public ChaseState ChaseState;
    public Atackstate Atackstate;
    public bool canseeplayer;


    public override state RunCurrentState()
   {
    if(vida <= 0)
    {
        return ChaseState;
    }
    else
    {
        return this;
    }
    
    
   }

   /*public OnTriggerEnter(Collider other){
        if (other.tag=="Objetivo"){
            return Atackstate;
        }
        if (other.tag=="Player"){
            //Player.GetComponent<datos>().vida -=damage;
            Debug.Log("enemigo");
        }

    }*/

    
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
        Debug.Log("Vida: " + vida);
    }
}
}
