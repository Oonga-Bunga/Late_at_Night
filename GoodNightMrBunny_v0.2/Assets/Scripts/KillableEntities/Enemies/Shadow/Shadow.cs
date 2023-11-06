using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shadow
{
    public class Shadow : AKillableEntity
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        private bool _isAvoiding = false;
        private bool _isBeingLit = false;
        private bool _isStunned = false;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _speed;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isAvoiding)
            {
                //steering avoidance
            }
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
                    Stunned();
                    break;
            }
        }

        public void Stunned()
        {
            //play stunned animation
        }
    }
}

    
