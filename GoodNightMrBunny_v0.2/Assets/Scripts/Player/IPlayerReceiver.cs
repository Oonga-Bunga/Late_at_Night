using UnityEngine;
public interface IPlayerReceiver
{
    public enum InputType
    {
        Down,
        Hold,
        Up
    }

    public enum HoldableObjectType
    {
        None,
        Flashlight,
        ClayBalls,
        ClayBin
    }

    public void Move(Vector2 direction);
    public void Run(InputType runInput);
    public void Jump(InputType jumpInput);
    public void UseHeldObject(InputType useImput);
    public void Interact(InputType interactInput);
    public void ChangeHeldObject(HoldableObjectType objectType, bool dropPrefab, float initializationValue);
    public void DropObject();
}