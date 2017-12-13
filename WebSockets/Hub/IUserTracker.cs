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
        Task AddUserToRoom(string connectionId, string groupName);
        Task RemoveUserFromRoom(UserDetails user);
        Task<UserDetails> GetUserDetails(string connectionId);

        event Action<UserDetails[], string> UsersJoined;
        event Action<UserDetails[], string> UsersLeft;
    }
}
