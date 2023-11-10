using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AHoldableObject : MonoBehaviour, IHoldableObject
{
    protected PlayerController _player;
    [SerializeField] protected GameObject _droppedObject;
    [SerializeField] public IPlayerReceiver.HoldableObjectType _holdableObjectType;

    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public virtual void Use(IPlayerReceiver.InputType attackInput)
    {

    }

    public virtual void Drop(bool dropPrefab, float dropDistance, float sphereRaycastRadius, float minimumDistanceFromCollision, LayerMask groundLayer, float force = 0)
    {
        if (_droppedObject != null && dropPrefab)
        {
            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * dropDistance;
            Vector3 dropDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            if (Physics.SphereCast(_player.transform.position, sphereRaycastRadius, dropDirection, out hitInfo, dropDistance, groundLayer))
            {
                dropPosition = hitInfo.point - (dropDirection * minimumDistanceFromCollision);
            }

            GameObject objectInstance = Instantiate(_droppedObject, dropPosition, Camera.main.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            InitializeInstance(objectInstance);
        }
        
        gameObject.SetActive(false);
    }

    protected virtual void InitializeInstance(GameObject instance)
    {
        
    }

    public virtual void Initialize(float value)
    {
        
    }
}
