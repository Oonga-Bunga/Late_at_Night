using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Shadow
{
    public class Shadow : AMonster
    {
        #region Attributes

        [Header("Shadow Settings")]

        [Header("States")]

        [SerializeField] private float _fleeingSpeed = 10;
        private bool _isAvoiding = false;
        [SerializeField] private float _stunTime = 5;
        private bool _isStunned = false;
        [SerializeField] private float _fleeingBuffer = 1;
        private float _fleeingTime = 0;
        [SerializeField] private float _switchDirectionCooldown = 2f;
        private float _switchDirectionTime = 0f;

        [Header("Navigation")]

        [SerializeField] private float _rayOffset = 2.5f;
        [SerializeField] private float _obstacleDetectionDistance = 20;
        [SerializeField] private float _rotationalDamp = 0.5f;
        [SerializeField] private float _rotationSpeed = 10f;
        private bool _isHittingAWall = false;

        [SerializeField] private Transform _actualCenter;
        [SerializeField] private LayerMask _groundLayer;
        private Vector3 _target;
        public event Action OnDied;

        private const string _animatorIsWalking = "IsWalking";
        private const string _animatorIsFleeing = "IsFleeing";
        private const string _animatorIsSucking = "IsSucking";
        private const string _animatorIsStunned = "IsStunned";
        private const string _animatorAttack = "Attack";
        private const string _animatorDie = "Die";

        [SerializeField] private GameObject _stunnedEffect;

        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            _target = new Vector3(-9999, -9999, -9999);
        }

        private void Update()
        {
            if (_target != new Vector3(-9999, -9999, -9999))
            {

                _animator.SetBool(_animatorIsWalking, true);
                _rb.velocity = Vector3.zero;
                if (_isAvoiding)
                {
                    CalculateSteeringAvoidance();
                }
                else
                {
                    Turn();
                }
                Move();
            }
            else
            {
                _animator.SetBool(_animatorIsWalking, false);
                _rb.velocity = Vector3.zero;
            }

            if (_fleeingTime > 0)
            {
                _fleeingTime = Mathf.Max(0, _fleeingTime - Time.deltaTime);
                _switchDirectionTime = Mathf.Max(0, _switchDirectionTime - Time.deltaTime);

                if (_fleeingTime == 0)
                {
                    _switchDirectionTime = 0;
                    _animator.SetBool(_animatorIsFleeing, false);
                    _currentSpeed = _walkingSpeed;
                }
                else if (_switchDirectionTime == 0)
                {
                    GenerateRandomTarget();
                }
            }
        }

        public void GenerateRandomTarget()
        {
            // Generar un �ngulo aleatorio en radianes
            float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

            // Calcular las coordenadas x y z del nuevo punto en la circunferencia
            float newX = transform.position.x + 1000 * Mathf.Cos(angle);
            float newZ = transform.position.z + 1000 * Mathf.Sin(angle);

            // Mantener la misma posici�n en el eje y
            float newY = transform.position.y;

            // Devolver el nuevo punto
            _target = new Vector3(newX, newY, newZ);

            _switchDirectionTime = _switchDirectionCooldown;
        }

        public override void TakeHit(float damage, IKillableEntity.AttackSource source)
        {
            switch (source)
            {
                case IKillableEntity.AttackSource.Flashlight:
                    ChangeHealth(damage, true);
                    if (!_isStunned)
                    {
                        _animator.SetBool(_animatorIsFleeing, true);
                        _fleeingTime = 1;
                        _currentSpeed = _fleeingSpeed;
                    }
                    break;
                case IKillableEntity.AttackSource.ClayBall:
                case IKillableEntity.AttackSource.Rocket:
                    Stunned();
                    break;
            }
        }

        public void Stunned()
        {
            _animator.SetBool(_animatorIsStunned, true);
            _isStunned = true;
            _stunnedEffect.SetActive(true);
            Invoke("Recover", _stunTime);
        }

        private void Recover()
        {
            _animator.SetBool(_animatorIsStunned, false);
            _isStunned = false;
            _stunnedEffect.SetActive(false);
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
            OnDied?.Invoke();
        }

        private void Turn()
        {
            RaycastHit upHit;
            RaycastHit fowardHit;
            RaycastHit leftHit;
            RaycastHit rightHit;
            Vector3 raycastOffset = Vector3.zero;

            Vector3 up = _actualCenter.position + transform.up * _rayOffset * 2;
            Vector3 left = _actualCenter.position - transform.right * _rayOffset;
            Vector3 right = _actualCenter.position + transform.right * _rayOffset;

            Debug.DrawRay(up, transform.up * _obstacleDetectionDistance, Color.red);
            Debug.DrawRay(left, transform.forward * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(right, transform.forward * _obstacleDetectionDistance, Color.yellow);

            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            Vector3 p1 = transform.position + Vector3.up * (collider.radius);
            Vector3 p2 = p1 + Vector3.up * (collider.height - collider.radius * 2);
            bool fowardHasHit = Physics.CapsuleCast(p1, p2, collider.radius, transform.forward, out fowardHit, _obstacleDetectionDistance * 0.5f, _groundLayer);
            bool leftHasHit = Physics.Raycast(left, transform.forward, out leftHit, _obstacleDetectionDistance, _groundLayer);
            bool rightHasHit = Physics.Raycast(right, transform.forward, out rightHit, _obstacleDetectionDistance, _groundLayer);

            if (!fowardHasHit || Physics.Raycast(up, transform.up, out upHit, _obstacleDetectionDistance, _groundLayer))
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
                _isHittingAWall = true;
                _rb.velocity += transform.up * _currentSpeed;
            }

            if (raycastOffset != Vector3.zero)
            {
                transform.Rotate(raycastOffset * _rotationSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 pos = _target - transform.position;
                pos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(pos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationalDamp * Time.deltaTime);
            }
        }

        private void Move()
        {
            RaycastHit hit;
            RaycastHit hit2;

            Vector3 up = _actualCenter.position + transform.up * _rayOffset * 2;
            Vector3 down = _actualCenter.position - transform.up * _rayOffset * 2;
            Vector3 upward = _actualCenter.position + transform.forward * _rayOffset * 2;

            Debug.DrawRay(up, Vector3.Normalize(transform.forward + transform.up) * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(down, Vector3.Normalize(transform.forward - transform.up) * _obstacleDetectionDistance, Color.yellow);
            Debug.DrawRay(upward, transform.up * 1000, Color.yellow);

            if (_target.y > transform.position.y)
            {
                if (Physics.Raycast(upward, transform.up, out hit, 1000, _groundLayer))
                {
                    if (hit.distance < Mathf.Abs(_target.y - transform.position.y))
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
                        else
                        {
                            _rb.velocity += transform.up * _currentSpeed;
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
                if (hit.distance < _obstacleDetectionDistance * 0.8)
                {
                    _rb.velocity += transform.up * _walkingSpeed;
                }
                else if (_target.y > transform.position.y && !_isHittingAWall)
                {
                    if (Mathf.Abs(_target.y - transform.position.y) > 0.1f)
                    {
                        _rb.velocity += transform.up * _walkingSpeed;
                    }
                }
            }
            else if (Physics.Raycast(up, Vector3.Normalize(transform.forward + transform.up), out hit2, _obstacleDetectionDistance, _groundLayer))
            {
                if (hit2.distance < _obstacleDetectionDistance * 0.8)
                {
                    _rb.velocity -= transform.up * _walkingSpeed;
                }
                else if (_target.y < transform.position.y && !_isHittingAWall)
                {
                    if (Mathf.Abs(_target.y - transform.position.y) > 0.1f)
                    {
                        _rb.velocity -= transform.up * _walkingSpeed;
                    }
                }
            }
            else if (_target.y < transform.position.y && !_isHittingAWall)
            {
                if (Mathf.Abs(_target.y - transform.position.y) > 0.1f)
                {
                    _rb.velocity -= transform.up * _walkingSpeed;
                }
            }
            else if (_target.y > transform.position.y && !_isHittingAWall)
            {
                if (Mathf.Abs(_target.y - transform.position.y) > 0.1f)
                {
                    _rb.velocity += transform.up * _walkingSpeed;
                }
            }

            _isHittingAWall = false;
        }

        private void CalculateSteeringAvoidance()
        {
            Vector3 distanceFromPlayerNormalized = (_player.transform.position - transform.position).normalized;

            Vector2 perpendicular = Vector2.Perpendicular(new Vector2(distanceFromPlayerNormalized.x, distanceFromPlayerNormalized.z)).normalized * _walkingSpeed;
            Vector3 firstPoint = new Vector3(transform.position.x + perpendicular.x, transform.position.y, transform.position.z + perpendicular.y);
            Vector3 secondPoint = new Vector3(-firstPoint.x, -firstPoint.y, -firstPoint.z);

            float firstDistance = Vector3.Distance(firstPoint, Baby.Instance.transform.position);
            float secondDistance = Vector3.Distance(secondPoint, Baby.Instance.transform.position);
            float currentDistance = Vector3.Distance(transform.position, Baby.Instance.transform.position);

            Vector3 final = new Vector3(-9999999, -9999999, -9999999);

            if (firstDistance <= secondDistance && firstDistance < currentDistance)
            {
                final = firstPoint;
            }
            else if (secondDistance <= firstDistance && secondDistance < currentDistance)
            {
                final = secondPoint;
            }

            if (final.x != -9999999)
            {
                Vector3 direction = final - transform.position;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationalDamp * Time.deltaTime);
            }
            else
            {
                Vector3 pos = _target - transform.position;
                pos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(pos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationalDamp * Time.deltaTime);
            }
        }

        public void TurnToTarget(Vector3 targetPos)
        {
            targetPos.y = transform.position.y;
            transform.DOLookAt(targetPos, 1);
        }
    }
}

    
