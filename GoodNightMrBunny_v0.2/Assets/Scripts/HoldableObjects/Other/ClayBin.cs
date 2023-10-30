using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ClayBin : AHoldableObject
{
    [SerializeField] private float _launchForce = 20f;

    void Start()
    {
        _holdableObjectType = IPlayerReceiver.HoldableObjectType.ClayBin;
    }

    public override void Use(IPlayerReceiver.InputType attackInput)
    {
        _player.ChangeHeldObject(IPlayerReceiver.HoldableObjectType.None, false);

        Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * _player.DropDistance;
        Vector3 dropDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.SphereCast(_player.transform.position, _player.SphereRaycastRadius, dropDirection, out hitInfo, _player.DropDistance, _player.GroundLayer))
        {
            dropPosition = hitInfo.point - (dropDirection * _player.MinimumDistanceFromCollision);
        }

        GameObject objectInstance = Instantiate(_droppedObject, dropPosition, Camera.main.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        objectInstance.GetComponent<Rigidbody>().AddForce(dropDirection * _launchForce, ForceMode.Impulse);
    }
}
