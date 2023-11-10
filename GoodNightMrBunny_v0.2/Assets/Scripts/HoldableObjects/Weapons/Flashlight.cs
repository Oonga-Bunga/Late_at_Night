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

    private static Flashlight _instance;

    public static Flashlight Instance => _instance;

    [SerializeField] private float _maxCharge = 100f;
    [SerializeField] private float _baseDamage = 5f;
    //Se multiplica al time.fixedDeltaTime para controlar el tiempo de descarga de la linterna
    [SerializeField] private float _dischargeMultiplier = 5f; 
    [SerializeField] private Light _spotlight;
    private bool _lightOn = false;
    private float _currentCharge;

    #endregion
    
    #region Methods
    public float CurrentCharge {
        get { return _currentCharge; }
    }

    public float MaxCharge
    {
        get { return _maxCharge; }
    }

    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = this;
        }

        _holdableObjectType = IPlayerReceiver.HoldableObjectType.Flashlight;
        _currentCharge = _maxCharge;
    }

    public override void Initialize(float charge)
    {
        _lightOn = false;
        _spotlight.enabled = false;
        _currentCharge = charge;
    }

    /// <summary>
    /// Mientras mantiene presionado enciende la luz de la linterna, al soltar, se apaga
    /// </summary>
    /// <param name="attackInput"></param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (_currentCharge < 0) return;
        
        if (attackInput == IPlayerReceiver.InputType.Down)
        {
            _lightOn = true;
            _spotlight.enabled = true;
        }

        if (attackInput == IPlayerReceiver.InputType.Up)
        {
            _lightOn = false;
            _spotlight.enabled = false;
        }
    }

    /// <summary>
    /// Mientras la linterna esté activa, lanza ataque si encuentra a enemigo y se descarga con el uso
    /// </summary>
    private void Update()
    {
        if (!_lightOn) return;
        
        HitEnemy();
        
        //Descarga de la linterna
        if(_currentCharge > 0)
        {
            _currentCharge -= Time.deltaTime * _dischargeMultiplier;
        }
        else
        {
            _lightOn = false;
            _spotlight.enabled = false;
        }

        //Parpadeo de la linterna
        if (_currentCharge < 8)
        {
            _spotlight.intensity = (int)_currentCharge % 2 == 0 ? 1f : 0f;
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
            // Obtener el componente del script del enemigo.
            AMonster enemy = hit.collider.GetComponent<AMonster>();
            if (enemy != null)
            {
                //Debug.Log("daño enemigo");
                enemy.TakeHit(_baseDamage * Time.deltaTime, IKillableEntity.AttackSource.Flashlight);
            }
            
        }
    }

    protected override void InitializeInstance(GameObject instance)
    {
        instance.GetComponent<AInteractable>().Initialize(_currentCharge);
    }

    #endregion
}
