using UnityEngine;
public interface IPlayerReceiver
{
    public enum InputType
    {
        Down,
        Hold,
        Up
    }

    public enum EquippableObjectType
    {
        Empty,
        Flashlight,
        ClayBalls,
        ClayBin
    }

    public void Move(Vector2 direction);
    public void Run(InputType runInput);
    public void Jump(InputType jumpInput);
    public void UseEquippedObject(InputType useImput);
    public void Interact(InputType interactInput);
    public void ChangeEquippedObject(EquippableObjectType pickupType);
    public void DropWeapon();
}