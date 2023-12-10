using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace EvilBunny
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EvilBunny : AMonster
    {
        private NavMeshAgent _agent;
        public event Action OnDied;

        // Animator strings
        private const string _animatorIsWalking = "IsWalking";
        private const string _animatorJump = "Jump";
        private const string _animatorAttack = "Attack";
        private const string _animatorMerge = "Merge";
        private const string _animatorDie = "Die";

        [SerializeField] private AudioSource _deathSound02;

        

        protected override void Awake()
        {
            base.Awake();

            _agent = GetComponent<NavMeshAgent>();
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
                case IKillableEntity.AttackSource.Flashlight:
                    ChangeHealth(damage, true);
                    break;
                case IKillableEntity.AttackSource.ClayBall:
                case IKillableEntity.AttackSource.Rocket:
                    ChangeHealth(MaxHealth, true);
                    _deathSound02.Play();
                    break;
            }
        }

        public override void Die()
        {
            OnDied?.Invoke();
        }

        public void Merge()
        {
            OnDied?.Invoke();
        }

        public void TurnToTarget(Vector3 targetPos)
        {
            targetPos.y = transform.position.y;
            transform.DOLookAt(targetPos, 1);
        }
    }
}

    
