using Microsoft.AspNetCore.SignalR;

namespace AppServer.Hubs
{
    public class UserHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Clients(Context.ConnectionId).SendAsync("ConnectSuccess");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
