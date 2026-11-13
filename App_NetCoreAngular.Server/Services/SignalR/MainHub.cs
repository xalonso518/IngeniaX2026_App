using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace App_NetCoreAngular.Server.Services.SignalR
{
    [Authorize]
    public class MainHub : Hub
    {
        // Métodos llamados al detectar conexión con cliente
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier ?? Context.ConnectionId;
            Console.WriteLine($"Client connected UserIdentifier: {Context.UserIdentifier}");
            Console.WriteLine($"Client connected ConnectionId: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Allow clients to subscribe to specific notification channels
        public async Task SubscribeToChannel(string channel)
        {
            // Add the connection to a SignalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, channel);
            Console.WriteLine($"Connection {Context.ConnectionId} subscribed to {channel}");
        }

        // Allow clients to unsubscribe from a channel
        public async Task UnsubscribeFromChannel(string channel)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
        }

        // Called when a client disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
