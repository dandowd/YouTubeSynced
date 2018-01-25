using WebSockets.PresenceHub;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebSockets.UserTracker;
using System.Linq;

namespace WebSockets
{
    public class Authenticator: IAuthenticator
    {
        internal class WaitingUser
        {
            public WaitingUser(string username, string groupname)
            {
                Username = username;
                Groupname = groupname;
            }
            public string Username { get; set; }
            public string Groupname { get; set; }
        }

        private IUserTracker<Chat> _userTracker;
        private ConcurrentDictionary<Guid, WaitingUser> _usersWaiting = new ConcurrentDictionary<Guid, WaitingUser>();

        public Authenticator(IUserTracker<Chat> tracker)
        {
            _userTracker = tracker;
        }

        public async Task<string> PassUser(HubConnectionContext connection, Guid waitingGuid)
        {
            var userInfo = _usersWaiting.Where(u => u.Key == waitingGuid).Select(u => u.Value).FirstOrDefault();

            await _userTracker.AddOrUpdateUser(connection, new UserDetails(connection.ConnectionId, userInfo.Username));

            await _userTracker.AddUserToRoom(connection, userInfo.Groupname);

            return userInfo.Groupname;
        }

        public async Task<Guid> AddUserToWaiting(string username, string groupname)
        {
            var result = await IsUserInRoom(username, groupname);
            if (!result)
            {
                var guid = Guid.NewGuid();

                _usersWaiting.TryAdd(guid, new WaitingUser(username, groupname));

                return guid;
            }

            return Guid.Empty;
        }

        private Task<bool> IsUserInRoom(string username, string groupname)
        {
            return _userTracker.CheckForUserInRoom(username, groupname);
        }
    }
}
