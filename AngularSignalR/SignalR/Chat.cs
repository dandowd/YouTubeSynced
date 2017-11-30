using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebSockets;

namespace AngularSignalR.SignalR
{
    public class Chat : HubWithPresence
    {
        public Chat(IUserTracker<HubWithPresence> userTracker) : base(userTracker) { }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

            await base.OnConnectedAsync();
        }
        
        public async Task SignInAsync(string userName, string groupName)
        {
            await UpdateUser(Context.Connection, new UserDetails(Context.ConnectionId, userName));

            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public override Task OnUsersJoined(UserDetails[] users, string groupName)
        {
            return Clients.Group(groupName).InvokeAsync("UsersJoined", users);
        }

        public override Task OnUsersLeft(UserDetails[] users, string groupName)
        {
            return Clients.Group(groupName).InvokeAsync("UsersLeft", users);
        }

        public async Task Send(string message, string groupName)
        {
            await Clients.Group(groupName).InvokeAsync("Send", message);
        }
    }
}
