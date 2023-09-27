using UnityEngine;

public interface IEquippableObject
{
    public void Use(IPlayerReceiver.InputType attackInput);
    public void Drop();
}
