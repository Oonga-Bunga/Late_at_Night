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

        gameObject.SetActive(false);
    }

    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        _player.DropHeldObject();
    }

    protected override void InitializeInstance(GameObject instance)
    {
        instance.GetComponent<Rigidbody>().AddForce(Vector3.forward * _launchForce, ForceMode.Impulse);
    }
}
