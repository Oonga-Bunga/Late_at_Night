using UnityEngine;

public class DropWeaponCommand : ACommand
{
    public DropWeaponCommand(IPlayerReceiver client) : base(client)
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
