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
    
    private float currentCharge;
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
        if (lightActive)
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
    }

    #endregion
}
