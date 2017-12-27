using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSockets
{
    public interface IUserTracker<out THub>
    {
        Task<IEnumerable<UserDetails>> UsersOnline();
        Task AddUser(HubConnectionContext connection, UserDetails userDetails);
        Task RemoveUser(HubConnectionContext connection);
        Task AddOrUpdateUser(HubConnectionContext connection, UserDetails user);
        Task AddUserToRoom(HubConnectionContext connection, string groupName);
        Task RemoveUserFromRoom(HubConnectionContext connection, UserDetails user);
        Task<UserDetails> GetUserDetails(string connectionId);

        event Action<UserDetails[], string, HubConnectionContext> UsersJoined;
        event Action<UserDetails[], string, HubConnectionContext> UsersLeft;
    }
}
