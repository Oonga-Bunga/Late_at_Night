using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitestingerTrap : MonoBehaviour
{
    [SerializeField] private float _explodeDistance = 5f;
    [SerializeField] private float _damage = 2f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _explosionEffect;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if (Vector3.Distance(playerPos, transform.position) <= _explodeDistance)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            PlayerController.Instance.PlayerHealth.TakeHit(_damage, IKillableEntity.AttackSource.KitestingerTrap);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
