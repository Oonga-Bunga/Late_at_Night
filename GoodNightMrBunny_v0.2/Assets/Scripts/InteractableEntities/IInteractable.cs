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
    public void InteractedPressAction();
    public void InteractedHoldAction();
    public void PlayerExitedRange();
    public void EnableOutline();
    public void DisableOutline();
}
