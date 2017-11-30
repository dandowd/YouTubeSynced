﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebSockets
{
    public class HubWithPresence : Hub
    {
        private IUserTracker<HubWithPresence> _userTracker;

        public event Action<HubConnectionContext, UserDetails> SignIn;

        public HubWithPresence(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        public Task<IEnumerable<UserDetails>> GetUsersOnline()
        {        
            return _userTracker.UsersOnline();
        }

        public Task AddUser(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.AddUser(connection, user);
        }
        
        public Task UpdateUser(HubConnectionContext connection, UserDetails user)
        {
            return _userTracker.UpdateUser(connection, user);
        }

        public virtual Task OnUsersJoined(UserDetails[] user, string groupName)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetails[] user, string groupName)
        {
            return Task.CompletedTask;
        }
    }
}
