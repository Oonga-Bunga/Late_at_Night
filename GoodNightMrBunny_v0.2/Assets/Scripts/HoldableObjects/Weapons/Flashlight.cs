using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Flashlight : AWeapon
{
    #region Atributtes
    
    public float currentCharge;
    static public float maxCharge = 100f;
    public float lanternDamage = 5f;
    public float range = 20f;
    public Light lanternLight;
    public bool lightActive = false;

    #endregion
    
    #region Methods
    public float CurrentCharge
    {
        get { return currentCharge; }
    }


    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.Flashlight;
        currentCharge = maxCharge;
        lightActive = false;
        lanternLight.enabled = false;
    }

    public override void Initialize(float charge)
    {
        currentCharge = charge;
    }

    /// <summary>
    /// Activa o desactiva la luz de la linterna
    /// </summary>
    public void ActiveLantern()
    {
        if (currentCharge < 0) return;
        lightActive = !lightActive;
        lanternLight.enabled = lightActive;
    }

    /// <summary>
    /// Llama a la función Active Lantern cuando se ejecuta la accion AttackInput
    /// </summary>
    /// <param name="attackInput"></param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (attackInput == IPlayerReceiver.InputType.Down)
        {
            ActiveLantern();
        }
    }

    /// <summary>
    /// Mientras la linterna esté activa
    /// </summary>
    private void FixedUpdate()
    {
        if (!lightActive) return;
        {
            HitEnemy();
            if(currentCharge > 0)
            {
                currentCharge -= Time.fixedDeltaTime*10;
            }
            else
            {
                lightActive = false;
                lanternLight.enabled = false;
            }
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
                    enemy.TakeHit(lanternDamage);
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

            Instantiate(droppedObject, dropPosition, Camera.main.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        }

        gameObject.SetActive(false);
    }

    #endregion
}
