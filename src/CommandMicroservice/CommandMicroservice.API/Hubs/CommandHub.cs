using Microsoft.AspNetCore.SignalR;

namespace CommandMicroservice.API.Hubs
{
    public class CommandHub : Hub
    {
        public async Task SendWarning(string message)
        {
            await Clients.All.SendAsync("ReceivedMsg", message);
        }

    }
}
