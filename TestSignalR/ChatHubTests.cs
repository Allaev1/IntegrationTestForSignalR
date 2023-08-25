using BlazorSignalR.Shared.HubMessages;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestSignalR
{
    public class ChatHubTests
    {
        [Fact]
        public async Task ShouldNotifySubscribers()
        {
            // Arrange
            var fixture = new AppFixture();

            // 1. Connect to the TestHub on {appUrl}/testHub
            var connection = await TestHelpers.StartConnectionAsync(fixture.GetCompleteServerUrl("/testhub"));

            // Using a mock handler so we can make use of the Verify method
            var mockHandler = new Mock<Action<Notification>>();
            var notificationToSend = new Notification { Message = "test message" };
            connection.On(nameof(Notification), mockHandler.Object);

            // Act
            using (var httpClient = new HttpClient())
            {
                // 2. Submit a POST request on {appUrl}/hub/test with a valid message
                await httpClient.PostAsJsonAsync(fixture.GetCompleteServerUrl("/hub/test"), notificationToSend);
            }

            // Assert
            // 3. Verify that a correct message was received
            mockHandler.Verify(x => x(It.Is<Notification>(n => n.Message == notificationToSend.Message)), Times.Once());
        }
    }
}
