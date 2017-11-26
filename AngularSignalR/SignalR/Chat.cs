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
        
        public async Task SignInAsync(string userName)
        {
            await AddUser(Context.Connection, new UserDetails(Context.ConnectionId, userName));
        }

        public async Task AddGroupAsync(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersJoined", users);
        }

        public override Task OnUsersLeft(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersLeft", users);
        }

        public async Task Send(string message, string groupName)
        {
            await Clients.Group(groupName).InvokeAsync("Send", message);
        }
    }
}
