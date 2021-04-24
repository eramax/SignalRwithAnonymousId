using Microsoft.AspNetCore.SignalR;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Lib
{
    public class SignalRHub : Hub
    {
        public static ConcurrentDictionary<string, List<string>> ConnectedUsers = new();

        public string GetMyToken()
        {
            var us = ConnectedUsers.Where(user => user.Value.Contains(Context.ConnectionId)).FirstOrDefault();
            return us.Key;
        }

        public override Task OnConnectedAsync()
        {
            var httpCtx = Context.GetHttpContext();
            var feature = httpCtx.Features.Get<IAnonymousIdFeature>();
            var userName = (feature != null && feature.AnonymousId != "")? feature.AnonymousId : Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            // Try to get a List of existing user connections from the cache
            var existingUserConnectionIds = ConnectedUsers.TryGetValue(userName, out List<string> lst) ? lst : new();

            // First add to a List of existing user connections (i.e. multiple web browser tabs)
            existingUserConnectionIds.Add(Context.ConnectionId);

            // Add to the global dictionary of connected users
            ConnectedUsers.TryAdd(userName, existingUserConnectionIds);
            Console.WriteLine($"{userName} on {Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }
        public void SendMessage(string name, string message)
        {
            Clients.All.SendAsync("ReceiveMessage", name, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var us = ConnectedUsers.Where(user => user.Value.Contains(Context.ConnectionId)).FirstOrDefault();
            if (ConnectedUsers.TryGetValue(us.Key, out var existingUserConnectionIds))
            {
                existingUserConnectionIds.Remove(Context.ConnectionId);
                if (existingUserConnectionIds.Count == 0) ConnectedUsers.TryRemove(us.Key, out _);
            }

            Clients.All.SendAsync("Users", "system", $"{Context.ConnectionId} left the conversation");
            Console.WriteLine($"{us.Key} on {Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}