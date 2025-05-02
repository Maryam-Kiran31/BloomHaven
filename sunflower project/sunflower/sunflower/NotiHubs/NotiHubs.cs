using Microsoft.AspNetCore.SignalR;

namespace sunflower.NotiHubs
{
    public class NotifyHubs : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
