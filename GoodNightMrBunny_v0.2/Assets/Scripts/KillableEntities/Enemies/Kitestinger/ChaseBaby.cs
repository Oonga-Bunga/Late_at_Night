using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBaby : MonoBehaviour
{
    private Transform objetivo; // El objeto al que queremos apuntar
    [SerializeField] private float velocidadMovimiento = 10f; // Velocidad de movimiento constante
    [SerializeField] private float velocidadRotacion = 120f; // Velocidad de rotaci�n

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        objetivo = Baby.Instance.transform;
        transform.rotation = Quaternion.LookRotation(objetivo.position - transform.position);

        // Asegurarse de que el Rigidbody tenga restricciones en rotaci�n para que no se caiga hacia un lado
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Rotaci�n para apuntar al objetivo
        RotarHaciaObjetivo();
    }

    private void FixedUpdate()
    {
        // Movimiento hacia adelante
        MoverHaciaAdelante();
    }

    private void MoverHaciaAdelante()
    {
        // Obtener la direcci�n hacia adelante del objeto
        Vector3 direccionAdelante = transform.forward;

        // Mover el objeto utilizando el Rigidbody
        rb.velocity = direccionAdelante * velocidadMovimiento;
    }

    private void RotarHaciaObjetivo()
    {
        if (objetivo != null)
        {
            // Determinar la direcci�n hacia el objetivo
            Vector3 direccionAlObjetivo = objetivo.position - transform.position;

            // Calcular la rotaci�n hacia el objetivo
            Quaternion rotacionHaciaObjetivo = Quaternion.LookRotation(direccionAlObjetivo);

            // Rotar gradualmente hacia la direcci�n del objetivo
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionHaciaObjetivo, velocidadRotacion * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Baby>() == Baby.Instance)
        {
            Baby.Instance.TakeHit(1, IKillableEntity.AttackSource.KitestingerProjectile);
        }

        Destroy(gameObject);
    }
}
