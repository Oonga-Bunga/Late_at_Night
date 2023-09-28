using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEquippableObject : MonoBehaviour, IEquippableObject
{
    protected PlayerController player;
    [SerializeField] protected GameObject droppedObject;
    protected float dropDistance = 3f;
    [SerializeField] protected float sphereRaycastRadius = 0.5f;
    [SerializeField] protected float minimumDistanceFromCollision = 0.5f;
    [SerializeField] public IPlayerReceiver.EquippableObjectType pickupType;

    public virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public virtual void Use(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop()
    {
        if (droppedObject != null)
        {
            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * dropDistance;
            Vector3 dropDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            if (Physics.SphereCast(player.transform.position, sphereRaycastRadius, dropDirection, out hitInfo, dropDistance))
            {
                dropPosition = hitInfo.point - (dropDirection * minimumDistanceFromCollision);
            }

            Instantiate(droppedObject, dropPosition, Quaternion.identity);
        }
        
        gameObject.SetActive(false);
    }
}
