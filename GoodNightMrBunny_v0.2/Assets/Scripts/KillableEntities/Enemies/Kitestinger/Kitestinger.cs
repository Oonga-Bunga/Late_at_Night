using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kitestinger : AMonster
{
    private NavMeshAgent _agent;
    private CapsuleCollider _capsuleCollider;
    [SerializeField] private Transform _groundDetectionPoint;
    public event Action OnDied;

    // Animator strings
    private const string _animatorIsWalking = "IsWalking";

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _agent.speed = _walkingSpeed;
    }

    private void Update()
    {
        _animator.SetBool(_animatorIsWalking, _agent.velocity.magnitude >= 0.1f);
    }

    public override void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        switch (source)
        {
            case IKillableEntity.AttackSource.Rocket:
                ChangeHealth(MaxHealth, true);
                break;
        }
    }

    public float CompareDistanceToPlayer()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        playerPos.y = transform.position.y;

        return Vector3.Distance(playerPos, transform.position);
    }

    public float CompareDistanceToBaby()
    {
        Vector3 babyPos = Baby.Instance.transform.position;
        babyPos.y = transform.position.y;

        return Vector3.Distance(babyPos, transform.position);
    }

    /*
    public float CompareDistanceToTraps()
    {
        Vector3 babyPos = Baby.Instance.transform.position;
        babyPos.y = transform.position.y;

        return Vector3.Distance(babyPos, transform.position);
    }
    */

    public override void Die()
    {
        OnDied?.Invoke();
    }

    public bool Landed()
    {
        RaycastHit hit;

        if (Physics.Raycast(_groundDetectionPoint.position, Vector3.down, out hit, 0.25f, LayerMask.GetMask("Ground")))
        {
            return true;
        }

        return false;
    }
}