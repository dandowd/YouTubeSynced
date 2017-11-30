using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSockets
{
    public class InMemoryUserTracker<THub> : IUserTracker<THub>
    {
        private readonly ConcurrentDictionary<HubConnectionContext, UserDetails> _usersOnline = new ConcurrentDictionary<HubConnectionContext, UserDetails>();
        private readonly ConcurrentDictionary<UserDetails, string> _groupMap = new ConcurrentDictionary<UserDetails, string>();

        public event Action<UserDetails[], string> UsersJoined;
        public event Action<UserDetails[], string> UsersLeft;

        public Task<IEnumerable<UserDetails>> UsersOnline() => Task.FromResult(_usersOnline.Values.AsEnumerable());


        public Task UpdateUser(HubConnectionContext connection, UserDetails user)
        {
            _usersOnline.AddOrUpdate(connection, user, (key, oldvalue) => user);

            return Task.CompletedTask;
        }

        public Task AddUser(HubConnectionContext connectionContext, UserDetails userDetails)
        { 
            _usersOnline.TryAdd(connectionContext, userDetails);
            
            return Task.CompletedTask;
        }

        public Task RemoveUser(HubConnectionContext connectionContext)
        {
            if (_usersOnline.TryRemove(connectionContext, out var userDetails))
            {
                //UsersLeft(new[] { userDetails });
            }

            return Task.CompletedTask;
        }

        public Task JoinGroup(UserDetails user, string groupName)
        {
            _groupMap.AddOrUpdate(user, groupName, (key, oldvalue) => groupName);

            UsersJoined(new[] { user }, groupName);

            return Task.CompletedTask;
        }

        public Task LeaveGroup(UserDetails user)
        {
            _groupMap.TryRemove(user, out var groupName);

            UsersLeft(new[] { user }, groupName);

            return Task.CompletedTask;
        }
    }
}
