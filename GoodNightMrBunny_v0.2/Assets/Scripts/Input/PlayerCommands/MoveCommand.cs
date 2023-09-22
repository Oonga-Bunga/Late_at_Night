using UnityEngine;

public class MoveCommand : ACommand
{
    public MoveCommand(IPlayerReceiver client) : base(client)
    {
    }

    public override void Execute()
    {
        Client.Move(new Vector2(0, 0));
    }

    public override void Execute(object data)
    {
        Client.Move((Vector2)data);
    }
}
