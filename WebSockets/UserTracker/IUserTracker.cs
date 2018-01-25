using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSockets
{
    public interface IUserTracker<out THub>
    {
        Task<IEnumerable<UserDetails>> UsersOnline(string groupname);
        Task RemoveUser(HubConnectionContext connection);
        Task AddOrUpdateUser(HubConnectionContext connection, UserDetails user);
        Task AddUserToRoom(HubConnectionContext connection, string groupName);
        Task RemoveUserFromRoom(HubConnectionContext connection, UserDetails user);
        Task<string> GetUsername(string connectionId);
        Task<bool> CheckForUserInRoom(string username, string groupName);

        event Action<UserDetails[], string, HubConnectionContext> UsersJoined;
        event Action<UserDetails[], string, HubConnectionContext> UsersLeft;
    }
}
