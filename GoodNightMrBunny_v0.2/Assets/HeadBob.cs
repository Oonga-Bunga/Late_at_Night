using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    //Source: https://www.youtube.com/watch?v=5MbR2qJK8Tc

    [SerializeField] private bool _enabled = true;
    [SerializeField] private bool _sidewaysBobToggle = true;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 500f)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;
    [SerializeField] private Transform _defaultCameraPos = null;

    private float _toggleSpeed = 1.0f;
    private Vector3 _startPos;
    private PlayerController _playerController;
    private Rigidbody _playerRb;

    private void Awake()
    {
        _playerController= GetComponent<PlayerController>();
        _playerRb = GetComponent<Rigidbody>();
        _startPos = _defaultCameraPos.localPosition;
    }

    private void LateUpdate()
    {
        _cameraHolder.position = _defaultCameraPos.position;

        if (!_enabled) return;

        CheckMotion();
        _camera.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(_playerRb.velocity.x, 0, _playerRb.velocity.z).magnitude;

        ResetPosition();

        if (speed < _toggleSpeed) return;
        if (!_playerController.IsPlayerGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency * _playerController.MaxCurrentSpeed) * _amplitude;

        if (!_sidewaysBobToggle) return pos;

        pos.x += Mathf.Cos(Time.time * (_frequency * _playerController.MaxCurrentSpeed) / 2) * _amplitude * 0.5f;

        return pos;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;

        _camera.localPosition = Vector3.Lerp(_camera.localPosition, Vector3.zero, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
}
