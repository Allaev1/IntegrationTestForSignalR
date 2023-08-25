using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSignalR
{
    public static class TestHelpers
    {
        public static async Task<HubConnection> StartConnectionAsync(string hubUrl)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            await connection.StartAsync();

            return connection;
        }
    }
}
