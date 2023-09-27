using UnityEngine;

public class InteractCommand : ACommand
{
    public InteractCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.Interact(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.Interact((IPlayerReceiver.InputType)data);
    }
}