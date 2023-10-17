using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Flashlight : AWeapon
{
    #region Atributtes

    public float currentCharge;
    static public float maxCharge = 100f;
    public float flashlightDamage = 5f;
    //Se multiplica al time.fixedDeltaTime para controlar el tiempo de descarga de la linterna
    public float dischargeMultiplier = 5f; 
    public float range = 20f;
    public Light spotlight;
    public bool lightOn = false;

    #endregion
    
    #region Methods
    public float CurrentCharge {
        get { return currentCharge; }
    }

    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.Flashlight;
        currentCharge = maxCharge;
        lightOn = false;
        spotlight.enabled = false;
        flashlightDamage = baseDamage;
    }

    public override void Initialize(float charge)
    {
        currentCharge = charge;
    }

    /// <summary>
    /// Mientras mantiene presionado enciende la luz de la linterna, al soltar, se apaga
    /// </summary>
    /// <param name="attackInput"></param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (currentCharge < 0) return;
        
        if (attackInput == IPlayerReceiver.InputType.Hold)
        {
            lightOn = true;
            spotlight.enabled = true;
        }

        if (attackInput == IPlayerReceiver.InputType.Up)
        {
            lightOn = false;
            spotlight.enabled = false;
        }
    }

    /// <summary>
    /// Mientras la linterna esté activa, lanza ataque si encuentra a enemigo y se descarga con el uso
    /// </summary>
    private void FixedUpdate()
    {
        if (!lightOn) return;
        
        HitEnemy();
        if(currentCharge > 0)
        {
            currentCharge -= Time.fixedDeltaTime*dischargeMultiplier;
        }
        else
        {
            lightOn = false;
            spotlight.enabled = false;
        }
        
    }

    /// <summary>
    /// Implementación del detectar al enemigo y hacerle daño mediante su TakeHit()
    /// </summary>
    private void HitEnemy()
    {
        // Lanzar un rayo desde la posición de la linterna en la dirección de la linterna.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Verificar si el objeto golpeado está en la capa "Enemy".
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Obtener el componente del script de enemigo.
                AMonster enemy = hit.collider.GetComponent<AMonster>();
                if (enemy != null)
                {
                    enemy.TakeHit(flashlightDamage);
                }
            }
        }
    }

    public override void Drop(bool dropPrefab, float dropDistance, float sphereRaycastRadius, float minimumDistanceFromCollision, LayerMask groundLayer)
    {
        if (droppedObject != null && dropPrefab)
        {
            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * dropDistance;
            Vector3 dropDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            if (Physics.SphereCast(player.transform.position, sphereRaycastRadius, dropDirection, out hitInfo, dropDistance, groundLayer))
            {
                dropPosition = hitInfo.point - (dropDirection * minimumDistanceFromCollision);
            }

            GameObject dropInstance = Instantiate(droppedObject, dropPosition, Camera.main.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            dropInstance.GetComponent<FlashlightPickup>().Initialize(currentCharge);
        }

        gameObject.SetActive(false);
    }

    #endregion
}
