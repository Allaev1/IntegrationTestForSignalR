using BlazorSignalR.Shared.HubMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSignalR
{
    public interface ITestHubDispatcher
    {
        Task Dispatch(Notification notification);
    }
}
