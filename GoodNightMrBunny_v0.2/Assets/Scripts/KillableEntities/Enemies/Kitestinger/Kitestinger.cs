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
    private List<Vector3> _nodeList = new List<Vector3>();

    // Animator strings
    private const string _animatorIsWalking = "IsWalking";

    [SerializeField] private AudioSource _deathSound02;
    [SerializeField] private GameObject _deathEffect;

    private float _stopTime = 0;

    public float StopTime => _stopTime;


    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _agent.speed = _walkingSpeed;
    }

    protected override void Start()
    {
        base.Start();

        CalculatePath();
    }

    private void Update()
    {
        _animator.SetBool(_animatorIsWalking, _agent.velocity.magnitude >= 0.1f);
        if (_agent.velocity.magnitude < 0.01f && !_animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowingTrap"))
        {
            _stopTime += Time.deltaTime;
        }
        else
        {
            _stopTime = 0;
        }
    }

    public override void TakeHit(float damage, IKillableEntity.AttackSource source)
    {
        switch (source)
        {
            case IKillableEntity.AttackSource.Rocket:
                ChangeHealth(MaxHealth, true);
                _deathSound02.Play();
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

    public float CompareDistanceToTraps()
    {
        GameObject[] trapList = GameObject.FindGameObjectsWithTag("KitestingerTrap");
        float bestDistance = 1000000;

        foreach (GameObject trap in trapList)
        {
            Vector3 trapPos = trap.transform.position;
            trapPos.y = transform.position.y;
            float distance = Vector3.Distance(trapPos, transform.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
            }
        }

        return bestDistance;
    }

    public override void Die()
    {
        Instantiate(_deathEffect, transform.position + Vector3.down, Quaternion.identity);
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

    private void CalculatePath()
    {
        Vector3 modifiedBabyPoint = ChangeVector3Y(Baby.Instance.transform.position);
        List<Vector3> closePoints = new List<Vector3>();

        foreach (Vector3 interestPoint in GameManager.InterestPointsListInstance)
        {
            Vector3 modifiedInterestPoint = ChangeVector3Y(interestPoint);

            Vector3 closestPoint = GetClosestPointOnLine(transform.position, modifiedBabyPoint, modifiedInterestPoint);

            if (Vector3.Distance(modifiedInterestPoint, closestPoint) < 20)
            {
                closePoints.Add(modifiedInterestPoint);
            }
        }

        Vector3 currentCalcPos = transform.position;

        while (closePoints.Count > 0)
        {
            float bestDistance = 100000;
            int bestPoint = -1;

            foreach (Vector3 closePoint in closePoints)
            {
                float distance = Vector3.Distance(closePoint, currentCalcPos);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestPoint = closePoints.IndexOf(closePoint);
                }
            }

            _nodeList.Add(closePoints[bestPoint]);
            currentCalcPos = closePoints[bestPoint];
            closePoints.RemoveAt(bestPoint);
        }

        _nodeList.Add(modifiedBabyPoint);
    }

    private Vector3 GetClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float t = Vector3.Dot(point - lineStart, lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        t = Mathf.Clamp01(t);
        Vector3 closestPoint = lineStart + t * lineDirection;
        return closestPoint;
    }

    private Vector3 ChangeVector3Y(Vector3 point)
    {
        return new Vector3(point.x, transform.position.y, point.z);
    }

    public Vector3 GetNextDestination()
    {
        Vector3 dest = _nodeList[0];
        _nodeList.RemoveAt(0);
        return dest;
    }
}
