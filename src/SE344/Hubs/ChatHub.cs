using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using SE344.Hubs.Clients;
using SE344.Models;

namespace SE344.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub()
        {
            _dbContext = Startup.ServiceProvider.GetService<ApplicationDbContext>();
            _userManager = Startup.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }

        public void SendMessage(string name, string message)
        {
            Clients.All.NewMessage(name, message);

            // persist to database
            var msg = new ChatMessage
            {
                Message = message,
                SentAt = DateTime.Now,
                SenderId = _userManager.Users.FirstOrDefault(user => name.Equals(user.Name))?.Id
            };
            _dbContext.ChatMessages.Add(msg);
            _dbContext.SaveChanges();
        }
    }
}
