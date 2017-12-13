using System;
using System.Collections.Generic;
using System.Text;

namespace WebSockets
{
    public class UserDetails
    {
        public UserDetails(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
            IsAdmin = false;
        }

        public UserDetails(string connectionId, string name, bool isAdmin)
        {
            ConnectionId = connectionId;
            Name = name;
            IsAdmin = isAdmin;
        }

        public string ConnectionId { get; }
        public string Name { get; }
        public bool IsAdmin { get; }
        public string RoomName { get; set; }
    }
}
