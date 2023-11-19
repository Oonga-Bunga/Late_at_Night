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
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Rigidbody _rb;

    void Update()
    {
        // Mueve el objeto hacia delante a una velocidad constante
        _rb.velocity = transform.up * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Crea el efecto de explosión en la posición del objeto
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector3.one * _blastRadius;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, -_blastRadius, _enemyLayer);
        foreach (Collider collider in hitColliders)
        {
            collider.GetComponent<AMonster>().TakeHit(_blastDamage, IKillableEntity.AttackSource.Rocket);
        }

        // Destruye el objeto actual
        Destroy(gameObject);
    }
}
