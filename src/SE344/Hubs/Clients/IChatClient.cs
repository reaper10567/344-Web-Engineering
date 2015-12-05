namespace SE344.Hubs.Clients
{
    public interface IChatClient
    {
        void NewMessage(string username, string message);
    }
}
