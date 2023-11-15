using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : AHoldableObject
{
    private static EmptyWeapon _instance;

    public static EmptyWeapon Instance => _instance;

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _holdableObjectType = IPlayerReceiver.HoldableObjectType.None;
    }
}
