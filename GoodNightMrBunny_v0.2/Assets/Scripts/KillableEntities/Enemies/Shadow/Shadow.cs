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
        [SerializeField] private float _stunTime = 5;
        private float _fleeingTime = 0;

        private const string _animatorIsWalking = "IsWalking";
        private const string _animatorIsFleeing = "IsFleeing";
        private const string _animatorIsSucking = "IsSucking";
        private const string _animatorIsStunned = "IsStunned";
        private const string _animatorAttack = "Attack";
        private const string _animatorDie = "Die";

        public float Speed => _speed;

        public float FleeingSpeed => _fleeingSpeed;

        private void Update()
        {
            _animator.SetBool(_animatorIsWalking, _agent.velocity.magnitude > 0.01f);

            if (_isAvoiding)
            {
                //steering avoidance
            }

            if (_fleeingTime > 0)
            {
                _fleeingTime = Mathf.Max(0, _fleeingTime - Time.deltaTime);

                if (_fleeingTime == 0)
                {
                    _animator.SetBool(_animatorIsFleeing, false);
                }
            }
        }

        public override void TakeHit(float damage, IKillableEntity.AttackSource source)
        {
            switch (source)
            {
                case IKillableEntity.AttackSource.Flashlight:
                    ChangeHealth(damage, true);
                    _animator.SetBool(_animatorIsFleeing, true);
                    _fleeingTime = 1;
                    break;
                case IKillableEntity.AttackSource.ClayBall:
                case IKillableEntity.AttackSource.Rocket:
                    Stunned();
                    break;
            }
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger(_animatorAttack);
        }

        public void Stunned()
        {
            _animator.SetBool(_animatorIsStunned, true);
            Invoke("Recover", _stunTime);
        }

        private void Recover()
        {
            _animator.SetBool(_animatorIsStunned, false);
        }

        public void StartAvoiding()
        {
            _isAvoiding = true;
        }

        public void StopAvoiding()
        {
            _isAvoiding = false;
        }

        public override void Die()
        {
            _animator.SetTrigger(_animatorDie);
            _hitbox.enabled = false;
        }
    }
}

    
