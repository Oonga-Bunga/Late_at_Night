using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private bool enabled = true;
    [SerializeField] private bool sidewaysBobToggle = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.0f;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;
    [SerializeField] private Transform defaultCameraPos = null;

    private float toggleSpeed = 1.0f;
    private Vector3 startPos;
    private PlayerController playerController;
    private Rigidbody playerRb;

    private void Awake()
    {
        playerController= GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        startPos = defaultCameraPos.localPosition;
    }

    private void LateUpdate()
    {
        cameraHolder.position = defaultCameraPos.position;
        cameraHolder.position = defaultCameraPos.localPosition + cameraHolder.position;

        if (!enabled) return;

        CheckMotion();
        camera.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude;

        ResetPosition();

        if (speed < toggleSpeed) return;
        if (!playerController.IsPlayerGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;

        if (!sidewaysBobToggle) return pos;

        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 0.5f;

        return pos;
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;

        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
}
