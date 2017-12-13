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

        public Task<UserDetails> GetUserDetails(string connectionId)
        {
            return Task.FromResult(_usersOnline.Where(c => c.Value.ConnectionId == connectionId).Select(u => u.Value).FirstOrDefault());
        }

        public Task AddOrUpdateUser(HubConnectionContext connection, UserDetails user)
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
            _usersOnline.TryRemove(connectionContext, out var userDetails);

            RemoveUserFromRoom(userDetails);

            return Task.CompletedTask;
        }

        public async Task AddUserToRoom(string connectionId, string groupName)
        {
            var user = await GetUserDetails(connectionId);
            user.RoomName = groupName;

            _groupMap.AddOrUpdate(user, groupName, (key, oldvalue) => groupName);

            UsersJoined(new[] { await GetUserDetails(connectionId) }, groupName);
        }

        public Task RemoveUserFromRoom(UserDetails user)
        {
            _groupMap.TryRemove(user, out var groupName);

            UsersLeft(new[] { user }, groupName);

            return Task.CompletedTask;
        }
    }
}
