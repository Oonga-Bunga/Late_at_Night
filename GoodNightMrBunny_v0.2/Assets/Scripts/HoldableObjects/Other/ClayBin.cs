using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ClayBin : AHoldableObject
{
    private static ClayBin _instance;

    public static ClayBin Instance => _instance;

    [SerializeField] private float _launchForce = 20f;

    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = this;
        }

        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBin;
    }

    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        _player.DropHeldObject(_launchForce);
    }
}
