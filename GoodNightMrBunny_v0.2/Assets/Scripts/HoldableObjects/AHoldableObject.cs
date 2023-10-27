using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AHoldableObject : MonoBehaviour, IHoldableObject
{
    protected PlayerController player;
    [SerializeField] protected GameObject droppedObject;
    [SerializeField] public IPlayerReceiver.HoldableObjectType holdableObjectType;

    public virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public virtual void Use(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop(bool dropPrefab, float dropDistance, float sphereRaycastRadius, float minimumDistanceFromCollision, LayerMask groundLayer, float initializationValue)
    {
        if (droppedObject != null && dropPrefab)
        {
            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * dropDistance;
            Vector3 dropDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            if (Physics.SphereCast(player.transform.position, sphereRaycastRadius, dropDirection, out hitInfo, dropDistance, groundLayer))
            {
                dropPosition = hitInfo.point - (dropDirection * minimumDistanceFromCollision);
            }

            GameObject objectInstance = Instantiate(droppedObject, dropPosition, Camera.main.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            objectInstance.GetComponent<AInteractable>().Initialize(initializationValue);
        }
        
        gameObject.SetActive(false);
    }

    public virtual void Initialize(float value)
    {
        
    }
}
