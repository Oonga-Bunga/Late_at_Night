using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _blastDamage = 5f;
    [SerializeField] private float _blastRadius = 5f;
    [SerializeField] private float _speed = 5f; // Velocidad de movimiento hacia arriba
    [SerializeField] private GameObject _explosionPrefab = null; // Prefab del efecto de explosión
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve el objeto hacia arriba a una velocidad constante
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) > 0) 
        {
            Explode();
        }
    }

    void Explode()
    {
        // Crea el efecto de explosión en la posición del objeto
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, -_blastRadius, _enemyLayer);
        foreach (Collider collider in hitColliders)
        {
            collider.gameObject.GetComponent<AMonster>().TakeHit(_blastDamage, GameManager.AttackSource.Rocket);
        }

        // Destruye el objeto actual
        Destroy(gameObject);
    }
}
