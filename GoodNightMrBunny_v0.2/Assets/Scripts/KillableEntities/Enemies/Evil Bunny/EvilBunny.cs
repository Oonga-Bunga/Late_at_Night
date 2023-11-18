using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EvilBunny
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EvilBunny : AMonster
    {
        private NavMeshAgent _agent;

        // Animator strings
        private const string _animatorIsWalking = "IsWalking";
        private const string _animatorJump = "Jump";
        private const string _animatorAttack = "Attack";
        private const string _animatorMerge = "Merge";
        private const string _animatorDie = "Die";

        protected override void Awake()
        {
            base.Awake();

            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _walkingSpeed;
        }

        private void Update()
        {
            _animator.SetBool(_animatorIsWalking, _agent.velocity.magnitude > 0.01f);
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
                    break;
            }
        }

        public override void Die()
        {
            Died?.Invoke(this, true);
            _animator.SetTrigger(_animatorDie);
            _hitbox.enabled = false;
        }
    }
}

    
