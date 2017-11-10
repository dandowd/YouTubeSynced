using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebSockets
{
    public class HubWithPresence : Hub
    {
        private IUserTracker<HubWithPresence> _userTracker;

        public HubWithPresence(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        public Task<IEnumerable<UserDetails>> GetUsersOnline()
        {        
            return _userTracker.UsersOnline();
        }

        public Task AddUser(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.AddUser(connection, user);
        }

        public virtual Task OnUsersJoined(UserDetails[] user)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetails[] user)
        {
            return Task.CompletedTask;
        }
    }
}
