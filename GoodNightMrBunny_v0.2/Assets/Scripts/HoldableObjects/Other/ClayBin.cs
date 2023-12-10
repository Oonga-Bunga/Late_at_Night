using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ClayBin : AHoldableObject
{
    private static ClayBin _instance;

    public static ClayBin Instance => _instance;

    [SerializeField] private float _launchForce = 20f;
    [SerializeField] private AudioSource _clayBinSound;

    protected void Awake()
    {
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
        _clayBinSound.Play();
    }

    protected override void InitializeInstance(GameObject instance)
    {
        instance.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * _launchForce, ForceMode.Impulse);
    }
}
