using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AWeapon
{
    #region Attributes
    
    private float currentBallNumber;
    static public int maxBallNumber = 6;
    public GameObject clayBallPrefab;
    public float shotForce = 20f;
    
    #endregion

    #region Methods
    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
        clayBallPrefab.GetComponent<ClayBallBehaviour>().baseDamage = baseDamage;
    }

    public override void Initialize(float ballNumber)
    {
        currentBallNumber = Mathf.Min(ballNumber, maxBallNumber);
    }

    /// <summary>
    /// Llama a la acción de disparar con el input del jugador
    /// </summary>
    /// <param name="attackInput">Input del jugador</param>
    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (currentBallNumber < 0) return;

        if(attackInput == IPlayerReceiver.InputType.Up)
        {
            Shot();
        }
    }

    /// <summary>
    /// Método que dispara bolas de plastilina
    /// </summary>
    public void Shot()
    {
        if (currentBallNumber <= 0) return;
        GameObject clayBall = Instantiate(clayBallPrefab, this.transform.position, Quaternion.identity);

        // Obtiene el Rigidbody de la nueva esfera.
        Rigidbody rb = clayBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calcula la direccion de lanzamiento.
            Vector3 direccionDeLanzamiento = transform.forward;

            // Aplica una fuerza al Rigidbody para lanzar la esfera.
            rb.AddForce(direccionDeLanzamiento * shotForce, ForceMode.Impulse);
            
            currentBallNumber -= 1;
        }
    }
    
    #endregion
}
