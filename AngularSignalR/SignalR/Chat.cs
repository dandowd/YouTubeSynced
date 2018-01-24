﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebSockets;

namespace AngularSignalR.SignalR
{
    public class Chat : HubWithPresence
    {
        public Chat(IUserTracker<Chat> userTracker) : base(userTracker) { }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

            await base.OnConnectedAsync();
        }
        
        public async Task SignInAsync(string userName, string groupName)
        {
            await SignInUser(Context.Connection, new UserDetails(Context.ConnectionId, userName));

            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveRoom(string userName, string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }

        public override Task OnUsersJoined(UserDetails[] users, string groupName)
        {
            return Clients.Group(groupName).InvokeAsync("UsersJoined", users);
        }

        public override Task OnUsersLeft(UserDetails[] users, string groupName)
        {
            return Clients.Group(groupName).InvokeAsync("UsersLeft", users);
        }

        public async Task Send(string message, string groupName)
        {
            await Clients.Group(groupName).InvokeAsync("Send", await GetUsername(Context.ConnectionId), message);
        }

        public async Task UserReady(string username, string groupName, bool isReady)
        {
            await Clients.Group(groupName).InvokeAsync("UserReady", username, isReady);
        }

        public async Task ChangeVideo(string groupName, string videoURL)
        {
            await Clients.Group(groupName).InvokeAsync("ChangeVideo", videoURL);
        }

        public async Task PlayToggle(string groupName, bool isPlaying)
        {
            await Clients.Group(groupName).InvokeAsync("PlayToggle", isPlaying);
        }
    }
}
