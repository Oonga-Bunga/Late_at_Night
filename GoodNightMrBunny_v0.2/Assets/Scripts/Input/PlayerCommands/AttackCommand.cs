using UnityEngine;

public class AttackCommand : ACommand
{
    public AttackCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.UseEquippedObject(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.UseEquippedObject((IPlayerReceiver.InputType)data);
    }
}