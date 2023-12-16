using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitestingerTrap : AKillableEntity
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
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public override void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        base.ChangeHealth(damage, true);

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        PlayerController.Instance.PlayerHealth.TakeHit(_damage, IKillableEntity.AttackSource.KitestingerTrap);
        Destroy(gameObject);
    }
}
