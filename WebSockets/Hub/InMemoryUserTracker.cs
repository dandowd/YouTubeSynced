﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockets
{
    public class InMemoryUserTracker<THub> : IUserTracker<THub>
    {
        private readonly ConcurrentDictionary<HubConnectionContext, UserDetails> _usersOnline = new ConcurrentDictionary<HubConnectionContext, UserDetails>();

        public event Action<UserDetails[]> UsersJoined;
        public event Action<UserDetails[]> UsersLeft;

        public Task<IEnumerable<UserDetails>> UsersOnline() => Task.FromResult(_usersOnline.Values.AsEnumerable());

        public Task AddUser(HubConnectionContext connectionContext, UserDetails userDetails)
        { 
            _usersOnline.TryAdd(connectionContext, userDetails);
            UsersJoined(new[] { userDetails });

            return Task.CompletedTask;
        }

        public Task RemoveUser(HubConnectionContext connectionContext)
        {
            if(_usersOnline.TryRemove(connectionContext, out var userDetails))
            {
                UsersLeft(new[] { userDetails });
            }

            return Task.CompletedTask;
        }
    }
}
