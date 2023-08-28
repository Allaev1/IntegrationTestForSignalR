using BlazorSignalR;
using BlazorSignalR.Shared;
using BlazorWebAssemblySignalRApp.Server.Hubs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;

namespace TestSignalR
{
    public class ChatHubTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChatHubTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldNotifySubscribers()
        {
            _factory.CreateClient(); // You need to call this procedure in order to create server
            var mockHandler = new Mock<Action<string, string>>(); 

            // Arrange

            var connection = await StartConnectionAsync(_factory.Server.CreateHandler(), SignalRConstants.ChatHubRouteName);
            //Register handler with some name, message will be sent from server to the handler with this name
            connection.On(SignalRConstants.SendMessageHandlerName, mockHandler.Object);

            var sourceUserName = "super_user";
            var sourceMessage = "Hello World!!";

            // Act
            // Send message to the ChatHub(to the server), to the SendMessage method(method with such name exist in ChatHub)
            await connection.InvokeAsync(nameof(ChatHub.SendMessage), sourceUserName, sourceMessage);

            // Assert
            // Make sure that handler was called once and that handler have recieved correct input values
            mockHandler.Verify(x => x(It.Is<string>(userName => userName == sourceUserName), It.Is<string>(message => message == sourceMessage)), Times.Once);
        }

        private async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string hubName)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/{hubName}", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }
    }
}
