using UnityEngine;

public interface IWeapon
{
    public void Attack(IPlayerReceiver.InputType attackInput);
    public void Drop();
}
