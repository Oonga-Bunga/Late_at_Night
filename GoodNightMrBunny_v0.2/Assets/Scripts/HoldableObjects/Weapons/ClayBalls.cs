using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBalls : AWeapon
{
    private float currentBallNumber;
    static public int maxBallNumber = 6;
    public GameObject clayBallPrefab;
    public float shotForce = 20f;

    void Start()
    {
        holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBalls;
    }

    public override void Initialize(float ballNumber)
    {
        currentBallNumber = Mathf.Min(ballNumber, maxBallNumber);
    }

    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        if (currentBallNumber < 0) return;

        if(attackInput == IPlayerReceiver.InputType.Up)
        {
            Shot();
        }
    }

    public void Shot()
    {
        if (currentBallNumber <= 0) return;
        GameObject clayBall = Instantiate(clayBallPrefab, this.transform.position, Quaternion.identity);

        // Obtiene el Rigidbody de la nueva esfera.
        Rigidbody rb = clayBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calcula la dirección de lanzamiento (hacia adelante en la escena).
            Vector3 direccionDeLanzamiento = transform.forward;

            // Aplica una fuerza al Rigidbody para lanzar la esfera.
            rb.AddForce(direccionDeLanzamiento * shotForce, ForceMode.Impulse);
            currentBallNumber -= 1;
        }
    }
}
