using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RotationPivot : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private GameObject _camera;
    private Vector3 _lastRot = new Vector3(0, 0, 0);

    private void Update()
    {
        if (_camera.transform.rotation.eulerAngles == _lastRot)
        {
            Debug.Log("clone");
        }
        else
        {
            Debug.Log("change");
        }

        _lastRot = _camera.transform.rotation.eulerAngles;

        transform.rotation = _camera.transform.rotation;
    }
}
