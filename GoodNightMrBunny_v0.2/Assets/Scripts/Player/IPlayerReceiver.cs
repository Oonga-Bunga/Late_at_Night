using UnityEngine;
public interface IPlayerReceiver
{
    public enum InputType
    {
        Down,
        Hold,
        Up
    }

    public enum WeaponType
    {
        Empty,
        Flashlight,
        Slingshot
    }

    public void Move(Vector2 direction);
    public void Run(InputType runInput);
    public void Jump(InputType jumpInput);
    public void Attack(InputType attackInput);
    public void Interact();
    public void ChangeWeapon(WeaponType weaponType);
    public void DropWeapon();
}