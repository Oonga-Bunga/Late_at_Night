using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float blastRadius = 5f;
    [SerializeField] private float speed = 5f; // Velocidad de movimiento hacia arriba
    [SerializeField] private GameObject explosionPrefab; // Prefab del efecto de explosión
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve el objeto hacia arriba a una velocidad constante
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0) 
        {
            Explode();
        }
    }

    void Explode()
    {
        // Crea el efecto de explosión en la posición del objeto
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Destruye el objeto actual
        Destroy(gameObject);
    }
}
