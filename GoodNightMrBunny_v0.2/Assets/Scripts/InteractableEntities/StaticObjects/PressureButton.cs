using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private RocketPlatform _rocketPlatform = null;

    void OnCollisionEnter(Collision collision)
    {
        // Comprueba si la colisión es con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            _rocketPlatform.LaunchRocket();
        }
    }
}
