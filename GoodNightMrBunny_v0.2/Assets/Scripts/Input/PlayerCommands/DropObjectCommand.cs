using UnityEngine;

public class DropObjectCommand : ACommand
{
    public DropObjectCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.DropObject();
    }

    public override void Execute(object data)
    {
        Client.DropObject();
    }
}
