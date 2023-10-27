using UnityEngine;

public interface IHoldableObject
{
    public void Use(IPlayerReceiver.InputType attackInput);
    public void Drop(bool dropPrefab, float dropDistance, float sphereRaycastRadius, float minimumDistanceFromCollision, LayerMask groundLayer, float initializationValue);
    public void Initialize(float value);
}
