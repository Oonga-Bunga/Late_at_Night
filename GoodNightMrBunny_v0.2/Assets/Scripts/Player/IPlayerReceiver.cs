using UnityEngine;
public interface IPlayerReceiver
{
    public enum InputType
    {
        Down,
        Hold,
        Up
    }

    public void Move(Vector2 direction);
    public void Jump(InputType jumpInput);
    public void Attack(InputType attackInput);
    public void Interact();
}