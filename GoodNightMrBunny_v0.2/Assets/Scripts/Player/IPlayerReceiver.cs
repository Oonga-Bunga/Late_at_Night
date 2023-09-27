using UnityEngine;
public interface IPlayerReceiver
{
    public enum InputType
    {
        Down,
        Hold,
        Up
    }

    public enum PickupType
    {
        Empty,
        Flashlight,
        ClayBalls,
        ClayBin
    }

    public void Move(Vector2 direction);
    public void Run(InputType runInput);
    public void Jump(InputType jumpInput);
    public void Attack(InputType attackInput);
    public void Interact(InputType interactInput);
    public void ChangeEquippedObject(PickupType pickupType);
    public void DropWeapon();
}