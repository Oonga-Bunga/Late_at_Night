using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private RocketPlatform rocketPlatform;

    void OnCollisionEnter(Collision collision)
    {
        // Comprueba si la colisi�n es con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            rocketPlatform.LaunchRocket();
        }
    }
}
