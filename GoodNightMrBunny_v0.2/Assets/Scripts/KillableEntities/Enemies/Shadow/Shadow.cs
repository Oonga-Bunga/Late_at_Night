using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shadow
{
    public class Shadow : AMonster
    {
        [SerializeField] private float _fleeingSpeed = 10;
        private bool _isAvoiding = false;
        private bool _isBeingLit = false;
        private bool _isStunned = false;

        public bool IsBeingLit => _isBeingLit;
        public bool IsStunned => _isStunned;

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

        public void PlayAttackAnimation()
        {

        }

        public void PlaySuccAnimation()
        {

        }

        public void Stunned()
        {
            //play stunned animation
        }

        public void StartAvoiding()
        {
            _isAvoiding = true;
            _agent.speed = _fleeingSpeed;
        }

        public void StopAvoiding()
        {
            _isAvoiding = false;
            _agent.speed = _speed;
        }

        public override void Die()
        {
            _hitbox.enabled = false;
        }
    }
}

    
