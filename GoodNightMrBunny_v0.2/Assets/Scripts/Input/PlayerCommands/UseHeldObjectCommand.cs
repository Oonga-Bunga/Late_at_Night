using UnityEngine;

public class UseHeldObjectCommand : ACommand
{
    public UseHeldObjectCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.UseHeldObject(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.UseHeldObject((IPlayerReceiver.InputType)data);
    }
}