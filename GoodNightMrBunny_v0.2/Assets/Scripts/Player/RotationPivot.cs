using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RotationPivot : MonoBehaviour
{
    public Camera playerCamera;

    void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
    }
}
