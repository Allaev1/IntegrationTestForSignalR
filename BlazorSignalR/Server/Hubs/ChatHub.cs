using BlazorSignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorWebAssemblySignalRApp.Server.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string userName, string message)
    {
        await Clients.All.SendAsync(SignalRConstants.SendMessageHandlerName, userName, message);
    }
}