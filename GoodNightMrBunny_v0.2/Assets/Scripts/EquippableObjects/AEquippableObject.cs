using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEquippableObject : MonoBehaviour, IEquippableObject
{
    private PlayerController player;
    [SerializeField] protected GameObject droppedObject;
    [SerializeField] public IPlayerReceiver.EquippableObjectType pickupType;

    public virtual void Use(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop()
    {
        if (droppedObject != null)
        {
            GameObject pickup = Instantiate(droppedObject);
        }
        
        gameObject.SetActive(false);
    }
}
