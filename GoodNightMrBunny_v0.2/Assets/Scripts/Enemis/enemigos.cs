using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemigos : MonoBehaviour
{
   public Transform Objetivo;
   public float Velocidad;
   public NavMeshAgent IA;

   public Animation Anim;
   public string AnimacionCaminar;
   public string AnimacionAtacar;

    void Update()
    {
        IA.speed = Velocidad;
        IA.SetDestination(Objetivo.position);
    /*
        if (IA.Velocidad == Vector3.zero){
            Anim.CrossFade(AnimacionAtacar);
        }
        else{
            Anim.CrossFade(AnimacionCaminar);
        }*/
    }
}
