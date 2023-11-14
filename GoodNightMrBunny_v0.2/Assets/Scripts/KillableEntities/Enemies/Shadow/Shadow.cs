using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shadow
{
    public class Shadow : AMonster
    {
        [SerializeField] private float _fleeingSpeed = 10;
        private float _currentSpeed = 0;
        private bool _isAvoiding = false;
        [SerializeField] private float _stunTime = 5;
        private float _fleeingTime = 0;

        private Transform _target;
        [SerializeField] private float _rayOffset = 2.5f;
        [SerializeField] private float _obstacleDetectionDistance = 20;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _rotationalDamp = 0.5f;
        [SerializeField] private float _rotationSpeed = 10f;
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

            _currentSpeed = _speed;
            _target = transform;
        }

        private void Update()
        {
            if (Vector3.Distance(_target.position, transform.position) > 0.01f)
            {
                _animator.SetBool(_animatorIsWalking, true);
                _rb.velocity = Vector3.zero;
                Turn();
                Move();
            }
            else
            {
                _animator.SetBool(_animatorIsWalking, false);
                _rb.velocity = Vector3.zero;
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
                    _currentSpeed = _speed;
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
                    _currentSpeed = _fleeingSpeed;
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
            RaycastHit fowardHit;
            RaycastHit leftHit;
            RaycastHit rightHit;
            Vector3 raycastOffset = Vector3.zero;

            Vector3 foward = _actualCenter.position + transform.forward * _rayOffset;
            Vector3 left = _actualCenter.position - transform.right * _rayOffset;
            Vector3 right = _actualCenter.position + transform.right * _rayOffset;

            Debug.DrawRay(left, transform.forward * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(right, transform.forward * _obstacleDetectionDistance, Color.yellow);

            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            Vector3 p1 = transform.position + Vector3.up * -collider.height * 0.5F;
            Vector3 p2 = p1 + Vector3.up * collider.height * 0.5F;
            bool fowardHasHit = Physics.CapsuleCast(p1, p2, collider.radius, transform.forward, out fowardHit, _obstacleDetectionDistance * 0.5f, _groundLayer);
            bool leftHasHit = Physics.Raycast(left, transform.forward, out leftHit, _obstacleDetectionDistance, _groundLayer);
            bool rightHasHit = Physics.Raycast(right, transform.forward, out rightHit, _obstacleDetectionDistance, _groundLayer);

            if (!fowardHasHit)
            {
                if (leftHasHit && rightHasHit)
                {
                    if (leftHit.distance < rightHit.distance)
                    {
                        raycastOffset += Vector3.up;
                    }
                    else
                    {
                        raycastOffset -= Vector3.up;
                    }
                }
                else if (leftHasHit)
                {
                    raycastOffset += Vector3.up;
                }
                else if (rightHasHit)
                {
                    raycastOffset -= Vector3.up;
                }
            }
            else
            {
                _rb.velocity += transform.up * _currentSpeed;
            }

            if (raycastOffset != Vector3.zero)
            {
                transform.Rotate(raycastOffset * _rotationSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 pos = _target.position - transform.position;
                pos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(pos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationalDamp * Time.deltaTime);
            }
        }

        private void Move()
        {
            RaycastHit hit;
            RaycastHit hit2;

            Vector3 foward = _actualCenter.position;
            Vector3 up = _actualCenter.position + transform.up * _rayOffset * 2;
            Vector3 down = _actualCenter.position - transform.up * _rayOffset * 2;
            Vector3 upward = _actualCenter.position + transform.forward * _rayOffset * 2;

            Debug.DrawRay(up, Vector3.Normalize(transform.forward + transform.up) * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(down, Vector3.Normalize(transform.forward - transform.up) * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(upward, transform.up * 1000, Color.yellow);

            if (_target.position.y > transform.position.y)
            {
                if (Physics.Raycast(upward, transform.up, out hit, 1000, _groundLayer))
                {
                    if (hit.distance < Mathf.Abs(_target.position.y - transform.position.y))
                    {
                        if (Physics.Raycast(_actualCenter.transform.position, transform.up, out hit2, 1000, _groundLayer))
                        {
                            if (Mathf.Abs(hit.distance - hit2.distance) < 0.01f)
                            {
                                _rb.velocity += transform.forward * _currentSpeed;
                            }
                            else
                            {
                                _rb.velocity += transform.up * _currentSpeed;
                            }
                        }
                    }
                    else
                    {
                        _rb.velocity += transform.forward * _currentSpeed;
                    }
                }
                else
                {
                    _rb.velocity += transform.forward * _currentSpeed;
                }
            }
            else
            {
                _rb.velocity += transform.forward * _currentSpeed;
            }

            if (Physics.Raycast(down, Vector3.Normalize(transform.forward - transform.up), out hit, _obstacleDetectionDistance, _groundLayer))
            {
                _rb.velocity += transform.up * _speed;
            }
            else if (Physics.Raycast(up, Vector3.Normalize(transform.forward + transform.up), out hit, _obstacleDetectionDistance, _groundLayer))
            {
                _rb.velocity -= transform.up * _speed;
            }
            else if (_target.position.y < transform.position.y)
            {
                _rb.velocity -= transform.up * _speed;
            }
            else if (_target.position.y > transform.position.y)
            {
                _rb.velocity += transform.up * _speed;
            }
        }

        private void CalculateSteeringAvoidance()
        {

        }
    }
}

    
