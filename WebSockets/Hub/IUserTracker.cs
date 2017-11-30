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
        Task UpdateUser(HubConnectionContext connection, UserDetails userDetails);

        event Action<UserDetails[], string> UsersJoined;
        event Action<UserDetails[], string> UsersLeft;
    }
}
