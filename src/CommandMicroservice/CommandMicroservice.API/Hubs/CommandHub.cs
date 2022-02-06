using Microsoft.AspNetCore.SignalR;

namespace CommandMicroservice.API.Hubs
{
    public class CommandHub : Hub
    {
        public async Task Notify()
        {
            await Clients.All.SendAsync("ReceivedMsg");
        }

    }
}
