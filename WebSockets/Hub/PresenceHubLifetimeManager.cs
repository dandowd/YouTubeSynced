using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSockets
{
    public class DefaultPresenceHublifetimeManager<THub> : PresenceHubLifetimeManager<THub, DefaultHubLifetimeManager<THub>>
    where THub : HubWithPresence
    {
        public DefaultPresenceHublifetimeManager(IUserTracker<THub> userTracker, IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
            : base(userTracker, serviceScopeFactory, loggerFactory, serviceProvider)
        {
        }
    }

    public class PresenceHubLifetimeManager<THub, THubLifetimeManager> : HubLifetimeManager<THub>, IDisposable
        where THubLifetimeManager : HubLifetimeManager<THub>
        where THub : HubWithPresence
    {
        private readonly HubConnectionList _connections = new HubConnectionList();
        private readonly IUserTracker<THub> _userTracker;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly HubLifetimeManager<THub> _wrappedHubLifetimeManager;
        private IHubContext<THub> _hubContext;

        public PresenceHubLifetimeManager(IUserTracker<THub> userTracker, IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _userTracker = userTracker;
            _userTracker.UsersJoined += OnUsersJoined;
            _userTracker.UsersLeft += OnUsersLeft;

            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<PresenceHubLifetimeManager<THub, THubLifetimeManager>>();
            _wrappedHubLifetimeManager = serviceProvider.GetRequiredService<THubLifetimeManager>();
        }

        public override async Task OnConnectedAsync(HubConnectionContext connection)
        {
            await _wrappedHubLifetimeManager.OnConnectedAsync(connection);
            _connections.Add(connection);
        }

        public override async Task OnDisconnectedAsync(HubConnectionContext connection)
        {
            await _wrappedHubLifetimeManager.OnDisconnectedAsync(connection);
            _connections.Remove(connection);

            await _userTracker.RemoveUser(connection);
        }

        private async void OnUsersJoined(UserDetails[] users, string groupName, HubConnectionContext connection)
        {
            await Notify(hub =>
            {
                if (users.Length == 1)
                {
                    return hub.OnUsersJoined(users, groupName);
                }
                else
                {
                    return hub.OnUsersJoined(users.Where(u => u.ConnectionId != hub.Context.Connection.ConnectionId).ToArray(), groupName);
                }
            }, connection);
        }

        private async void OnUsersLeft(UserDetails[] users, string groupName, HubConnectionContext connection)
        {
            await Notify(hub => hub.OnUsersLeft(users, groupName), connection);
        }

        private async Task Notify(Func<THub, Task> invocation, HubConnectionContext connection)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var hubActivator = scope.ServiceProvider.GetRequiredService<IHubActivator<THub>>();
                var hub = hubActivator.Create();

                if (_hubContext == null)
                {
                    // Cannot be injected due to circular dependency
                    _hubContext = _serviceProvider.GetRequiredService<IHubContext<THub>>();
                }

                hub.Clients = _hubContext.Clients;
                hub.Context = new HubCallerContext(connection);
                hub.Groups = _hubContext.Groups;
                try
                {
                    await invocation(hub);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Presence notification failed.");
                }
                finally
                {
                    hubActivator.Release(hub);
                }
            }
        }

        public void Dispose()
        {
        }

        public override Task InvokeAllAsync(string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.InvokeAllAsync(methodName, args);
        }

        public override Task InvokeAllExceptAsync(string methodName, object[] args, IReadOnlyList<string> excludedIds)
        {
            return _wrappedHubLifetimeManager.InvokeAllExceptAsync(methodName, args, excludedIds);
        }

        public override Task InvokeConnectionAsync(string connectionId, string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.InvokeConnectionAsync(connectionId, methodName, args);
        }

        public override Task InvokeGroupAsync(string groupName, string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.InvokeGroupAsync(groupName, methodName, args);
        }

        public override async Task InvokeUserAsync(string userId, string methodName, object[] args)
        {
            await _wrappedHubLifetimeManager.InvokeUserAsync(userId, methodName, args);
        }

        public override async Task AddGroupAsync(string connectionId, string groupName)
        {
            await _userTracker.AddUserToRoom(_connections.Where(u => u.ConnectionId == connectionId).FirstOrDefault(), groupName);

            await _wrappedHubLifetimeManager.AddGroupAsync(connectionId, groupName);
        }
        
        public override async Task RemoveGroupAsync(string connectionId, string groupName)
        {
            await _wrappedHubLifetimeManager.RemoveGroupAsync(connectionId, groupName);
        }
    }
}
