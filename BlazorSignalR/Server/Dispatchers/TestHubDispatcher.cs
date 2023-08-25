using BlazorSignalR.Server.Hubs;
using BlazorSignalR.Shared.HubMessages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSignalR
{
    public class TestHubDispatcher : ITestHubDispatcher
    {
        private readonly IHubContext<TestHub> _hubContext;

        public TestHubDispatcher(IHubContext<TestHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Dispatch(Notification notification)
        {
            return _hubContext.Clients.All.SendAsync(nameof(Notification), notification);
        }
    }
}
