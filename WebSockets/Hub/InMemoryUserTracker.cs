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

        public event Action<UserDetails[], string, HubConnectionContext> UsersJoined;
        public event Action<UserDetails[], string, HubConnectionContext> UsersLeft;

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

        public Task RemoveUser(HubConnectionContext connection)
        {
            _usersOnline.TryRemove(connection, out var userDetails);

            RemoveUserFromRoom(connection, userDetails);

            return Task.CompletedTask;
        }

        public async Task AddUserToRoom(HubConnectionContext connection, string groupName)
        {
            var user = await GetUserDetails(connection.ConnectionId);
            user.RoomName = groupName;

            _groupMap.AddOrUpdate(user, groupName, (key, oldvalue) => groupName);

            UsersJoined(new[] { user }, groupName, connection);
        }

        public Task RemoveUserFromRoom(HubConnectionContext connection, UserDetails user)
        {
            _groupMap.TryRemove(user, out var groupName);

            UsersLeft(new[] { user }, groupName, connection);

            return Task.CompletedTask;
        }
    }
}
