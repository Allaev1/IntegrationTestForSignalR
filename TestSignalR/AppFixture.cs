using BlazorSignalR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSignalR
{
    public class AppFixture
    {
        public const string BaseHttpsUrl = "https://localhost:7173";

        static AppFixture()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls(BaseHttpsUrl, "http://localhost:5029");
            Program.ConfigureServices(builder.Services);

            var app = builder.Build();
            Program.Configure(app);
            app.Start();
        }

        public string GetCompleteServerUrl(string route)
        {
            route = route?.TrimStart('/', '\\');
            return $"{BaseHttpsUrl}/{route}";
        }
    }
}
