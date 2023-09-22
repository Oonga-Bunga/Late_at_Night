using UnityEngine;

public class AttackCommand : ACommand
{
    public AttackCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.Attack(IPlayerReceiver.InputType.Down);
    }

    public override void Execute(object data)
    {
        Client.Attack((IPlayerReceiver.InputType)data);
    }
}