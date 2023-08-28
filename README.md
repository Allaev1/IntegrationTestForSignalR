# IntegrationTestForSignalR
Tutorial on how to write integration test for SignalR hub. Client is Blazor

## Instructions

1. Complete this tutorial - https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-7.0&tabs=visual-studio&pivots=webassembly

> Before going to next step I recommend you place literals related to signalr to constants in my case  
I created `SignalRConstants` class with two constants. First one is `SendMessageHandlerName` and second one is  
`ChatHubRouteName`

2. Add test project. In my case I added xUnit test project
3. Add link to your server project. In my case I add link to the `BlazorSignalR.Server`
4. Add this list of NuGet packages: 
- Moq
- FluentAssertion
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.AspNetCore.SignalR.Client
5. Add test class with name - ***ChatHubTest***
> **IMPORTANT NOTE**: All next code changes will take place in one class, in `ChatHubTest` class

6. Derive class from `IClassFixture<WebApplicationFactory<Program>>` so your class will look like this:
```
public class ChatHubTests : IClassFixture<WebApplicationFactory<Program>>
{}
```
> `Program` class is the entry point of my `BlazorSignalR.Server` project

> By implementing `IClassFixture<T>` interface you create one context of `T` for whole tests in the current class  
more you can read here - https://xunit.net/docs/shared-context#class-fixture  

> `WebApplicationFactory<TProgram>` is the class that create the system that will be under the test, where   
`TProgram` is the class that contains the startup logic of your application. There is a big reason why you should you use it,
please read [this](https://medium.com/executeautomation/integration-testing-of-net-7-asp-net-apps-with-minimal-hosting-model-1ac87ed5edc5) article about why you should consider to 
use this class if you do integration test on .NET 7

7. Add constructor to your class. Constructor should accept `WebApplicationFactory<Program> factory` so your code will be like this:
```
public class ChatHubTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ChatHubTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
}
```

8. Add `StartConnectionAsync` method that create connection with SignlaR hub - `ChatHub`. Result:
```
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
```

> You can see that url contains "ws" instead of "https". This is important since signlar prefered way of communication is web socket

9. Add the test:
```
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
```