using UnityEngine;

public class RunCommand : ACommand
{
    public RunCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.Run(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.Run((IPlayerReceiver.InputType)data);
    }
}