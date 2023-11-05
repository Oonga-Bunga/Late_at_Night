using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Flashlight : AHoldableObject
{
    #region Atributtes

    static public float maxCharge = 100f;
    [SerializeField] private float baseDamage = 5f;
    //Se multiplica al time.fixedDeltaTime para controlar el tiempo de descarga de la linterna
    [SerializeField] private float dischargeMultiplier = 5f; 
    [SerializeField] private Light spotlight;
    [SerializeField] private bool lightOn = false;
    [SerializeField] private float currentCharge;

    #endregion
    
    #region Methods
    public float CurrentCharge {
        get { return currentCharge; }
    }

    void Start()
    {
        _holdableObjectType = IPlayerReceiver.HoldableObjectType.Flashlight;
        currentCharge = maxCharge;
    }

    public override void Initialize(float charge)
    {
        lightOn = false;
        spotlight.enabled = false;
        currentCharge = charge;
    }

    /// <summary>
    /// Mientras mantiene presionado enciende la luz de la linterna, al soltar, se apaga
    /// </summary>
    /// <param name="attackInput"></param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        
        if (currentCharge < 0) return;
        
        if (attackInput == IPlayerReceiver.InputType.Down)
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
    private void Update()
    {
        if (!lightOn) return;
        
        HitEnemy();
        
        //Descarga de la linterna
        if(currentCharge > 0)
        {
            currentCharge -= Time.deltaTime*dischargeMultiplier;
        }
        else
        {
            lightOn = false;
            spotlight.enabled = false;
        }

        //Parpadeo de la linterna
        if (currentCharge < 8)
        {
            spotlight.intensity = (int)currentCharge % 2 == 0 ? 1f : 0f;
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
        
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy"))) // Filtra por la capa "Enemy"
        {
            //Debug.Log("daño enemigo");
            // Obtener el componente del script del enemigo.
            AKillableEntity enemy = hit.collider.GetComponent<AKillableEntity>();
            if (enemy != null)
            {
                Debug.Log("daño enemigo");
                enemy.TakeHit(baseDamage * Time.deltaTime, IKillableEntity.AttackSource.Flashlight);
            }
            
        }
    }

    protected override void InitializeInstance(GameObject instance)
    {
        instance.GetComponent<AInteractable>().Initialize(currentCharge);
    }

    #endregion
}
