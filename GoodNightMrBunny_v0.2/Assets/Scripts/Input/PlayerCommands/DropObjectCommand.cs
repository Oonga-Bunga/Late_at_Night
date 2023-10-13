using UnityEngine;

public class DropObjectCommand : ACommand
{
    public DropObjectCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.DropHeldObject();
    }

    public override void Execute(object data)
    {
        Client.DropHeldObject();
    }
}
