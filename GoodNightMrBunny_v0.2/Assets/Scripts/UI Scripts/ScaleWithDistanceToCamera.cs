using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithDistanceToCamera : MonoBehaviour
{
    [SerializeField] private float _unscaledDistance = 5f; // Distancia original de escala

    void Update()
    {
        float dist = Vector3.Distance(transform.position, Camera.main.transform.position);
        transform.localScale = Vector3.one * dist / _unscaledDistance;
    }
}
