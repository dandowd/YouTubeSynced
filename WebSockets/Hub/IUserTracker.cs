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

        event Action<UserDetails[]> UsersJoined;
        event Action<UserDetails[]> UsersLeft;
    }
}
