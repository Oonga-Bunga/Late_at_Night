using UnityEngine;

public interface IHoldableObject
{
    public void Use(IPlayerReceiver.InputType attackInput);
    public void Drop(bool dropPrefab);
    public void Initialize(float value);
}
