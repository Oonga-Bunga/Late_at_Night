using UnityEngine;

public class JumpCommand : ACommand
{
    public JumpCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.Jump(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.Jump((IPlayerReceiver.InputType)data);
    }
}
