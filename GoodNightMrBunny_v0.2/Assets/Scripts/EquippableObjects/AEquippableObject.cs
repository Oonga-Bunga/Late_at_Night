using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEquippableObject : MonoBehaviour, IEquippableObject
{
    private PlayerController player;
    [SerializeField] protected GameObject objectPrefab;

    public virtual void Use(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop()
    {
        if (objectPrefab != null)
        {
            GameObject pickup = Instantiate(objectPrefab);
        }
        
        gameObject.SetActive(false);
    }
}
