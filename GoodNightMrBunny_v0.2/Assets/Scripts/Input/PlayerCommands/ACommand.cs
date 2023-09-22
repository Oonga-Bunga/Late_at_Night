public abstract class ACommand : ICommand
{
    protected readonly IPlayerReceiver Client;

    protected ACommand(IPlayerReceiver client)
    {
        Client = client;
    }

    public abstract void Execute();

    public abstract void Execute(object data);
}