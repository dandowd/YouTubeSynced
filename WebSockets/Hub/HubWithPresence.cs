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

        public Task<string> GetUsername(string connectionId)
        {
            return _userTracker.GetUsername(connectionId);
        }

        public Task AddUser(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.AddUser(connection, user);
        }
        
        public Task SignInUser(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.AddOrUpdateUser(connection, user);
        }

        public Task AddUserToRoom(HubConnectionContext connection, string groupName)
        {
            return _userTracker.AddUserToRoom(connection, groupName);
        }

        public Task RemoveUserFromRoom(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.RemoveUserFromRoom(connection, user);
        }

        public virtual Task OnUsersJoined(UserDetails[] user, string groupName)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetails[] user, string groupName)
        {
            return Task.CompletedTask;
        }
    }
}
