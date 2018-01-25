using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebSockets.UserTracker
{
    public interface IAuthenticator
    {
        Task<string> PassUser(HubConnectionContext connection, Guid guid);
        Task<Guid> AddUserToWaiting(string username, string groupname);
    }
}
