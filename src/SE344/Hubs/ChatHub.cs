using Microsoft.AspNet.SignalR;
using SE344.Hubs.Clients;

namespace SE344.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.NewMessage(name, message);
        }
    }
}
