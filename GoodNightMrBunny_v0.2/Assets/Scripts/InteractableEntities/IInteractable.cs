using UnityEngine;

public interface IInteractable
{
    public void Interacted(object agent);

    public void EnableOutline();

    public void DisableOutline();
}
