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
        private bool isAvoiding = false;

        public Shadow(float health) : base(health)
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _speed;
        }

        private void Update()
        {
            if (isAvoiding)
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

        }
    }
}

    
