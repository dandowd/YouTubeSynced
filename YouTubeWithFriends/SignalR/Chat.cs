using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebSockets;

namespace YouTubeWithFriends.SignalR
{
    public class Chat : HubWithPresence
    {
        public Chat(IUserTracker<HubWithPresence> userTracker) : base(userTracker) { }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

            await base.OnConnectedAsync();
        }

        public async Task AddGroupAsync(string connectionId, string groupName)
        {
            await AddGroupAsync(connectionId, groupName);
        }

        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersJoined", users);
        }

        public override Task OnUsersLeft(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersLeft", users);
        }

        public async Task Send(string message)
        {
            
            await Clients.All.InvokeAsync("Send", message);
        }
    }
}
