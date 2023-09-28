using UnityEngine;

public interface IInteractable
{
    public enum InteractType
    {
        Press,
        Hold,
        PressAndHold
    }

    public void Interacted(IPlayerReceiver.InputType interactInput);
    public void PlayerExitedRange();
    public void EnableOutline();
    public void DisableOutline();
}
