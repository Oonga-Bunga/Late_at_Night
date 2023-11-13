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

        private Transform _target;
        [SerializeField] private float _rayOffset = 2.5f;
        [SerializeField] private float _obstacleDetectionDistance = 20;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _rotationalDamp = 0.5f;
        [SerializeField] private Transform _actualCenter;

        private const string _animatorIsWalking = "IsWalking";
        private const string _animatorIsFleeing = "IsFleeing";
        private const string _animatorIsSucking = "IsSucking";
        private const string _animatorIsStunned = "IsStunned";
        private const string _animatorAttack = "Attack";
        private const string _animatorDie = "Die";

        public float Speed => _speed;

        public float FleeingSpeed => _fleeingSpeed;

        public Transform Target
        {
            get { return _target; }
            set { _target = value; }
        }

        protected override void Awake()
        {
            base.Awake();

            _target = transform;
        }

        private void Update()
        {
            if (Vector3.Distance(_target.position, transform.position) > 0.01f)
            {
                _animator.SetBool(_animatorIsWalking, true);
                Pathfinding();
                Move();
            }
            else
            {
                _animator.SetBool(_animatorIsWalking, false);
            }

            if (_isAvoiding)
            {
                CalculateSteeringAvoidance();
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

        private void Turn()
        {
            Vector3 pos = _target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(pos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationalDamp * Time.deltaTime);
        }

        private void Move()
        {
            if (_fleeingTime > 0)
            {
                transform.position += transform.forward * _fleeingSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * _speed * Time.deltaTime;
            }
        }

        private void Pathfinding()
        {
            RaycastHit hit;
            Vector3 raycastOffset = Vector3.zero;

            Vector3 left = _actualCenter.position - transform.right * _rayOffset;
            Vector3 right = _actualCenter.position + transform.right * _rayOffset;
            Vector3 up = _actualCenter.position + transform.up * _rayOffset * 2;
            Vector3 down = _actualCenter.position - transform.up * _rayOffset * 2;

            Debug.DrawRay(left, transform.forward * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(right, transform.forward * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(up, transform.forward * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(down, transform.forward * _obstacleDetectionDistance, Color.yellow);

            if (Physics.Raycast(left, transform.forward, out hit, _obstacleDetectionDistance, _groundLayer))
            {
                raycastOffset += Vector3.right;
            }
            else if (Physics.Raycast(right, transform.forward, out hit, _obstacleDetectionDistance, _groundLayer))
            {
                raycastOffset -= Vector3.right;
            }

            if (Physics.Raycast(down, transform.forward, out hit, _obstacleDetectionDistance, _groundLayer))
            {
                raycastOffset += Vector3.up;
            }
            else if (Physics.Raycast(up, transform.forward, out hit, _obstacleDetectionDistance, _groundLayer))
            {
                raycastOffset -= Vector3.up;
            }

            if (raycastOffset != Vector3.zero)
            {
                transform.Rotate(raycastOffset * 20f * Time.deltaTime);
            }
            else
            {
                Turn();
            }
        }

        private void CalculateSteeringAvoidance()
        {

        }
    }
}

    
