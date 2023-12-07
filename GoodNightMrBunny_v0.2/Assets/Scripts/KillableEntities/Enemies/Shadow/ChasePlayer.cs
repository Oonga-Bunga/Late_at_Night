using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    private Transform objetivo; // El objeto al que queremos apuntar
    [SerializeField] private float velocidadMovimiento = 5f; // Velocidad de movimiento constante
    [SerializeField] private float velocidadRotacion = 120f; // Velocidad de rotación

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        objetivo = PlayerController.Instance.transform;

        // Asegurarse de que el Rigidbody tenga restricciones en rotación para que no se caiga hacia un lado
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Rotación para apuntar al objetivo
        RotarHaciaObjetivo();
    }

    private void FixedUpdate()
    {
        // Movimiento hacia adelante
        MoverHaciaAdelante();
    }

    private void MoverHaciaAdelante()
    {
        // Obtener la dirección hacia adelante del objeto
        Vector3 direccionAdelante = transform.forward;

        // Mover el objeto utilizando el Rigidbody
        rb.velocity = direccionAdelante * velocidadMovimiento;
    }

    private void RotarHaciaObjetivo()
    {
        if (objetivo != null)
        {
            // Determinar la dirección hacia el objetivo
            Vector3 direccionAlObjetivo = objetivo.position - transform.position;

            // Calcular la rotación hacia el objetivo
            Quaternion rotacionHaciaObjetivo = Quaternion.LookRotation(direccionAlObjetivo);

            // Rotar gradualmente hacia la dirección del objetivo
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionHaciaObjetivo, velocidadRotacion * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.Instance.PlayerHealth.TakeHit(1, IKillableEntity.AttackSource.Zanybell);
        }

        Destroy(gameObject);
    }
}
