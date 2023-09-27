using UnityEngine;

public interface IInteractable
{
    public enum InteractType
    {
        Press,
        Hold,
        PressAndHold
    }

    public void Interacted(PlayerController player, IPlayerReceiver.InputType interactInput);

    public void InteractedDown();

    public void InteractedUp();

    public void InteractedPressAction();

    public void InteractedHoldAction();

    public void EnableOutline();

    public void DisableOutline();
}
