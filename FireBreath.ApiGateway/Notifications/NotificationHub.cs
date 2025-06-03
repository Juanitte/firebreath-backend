using Microsoft.AspNetCore.SignalR;

namespace FireBreath.ApiGateway.Notifications
{
    public class NotificationHub : Hub
    {
        // Puedes definir grupos si necesitas
        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task LeaveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}
