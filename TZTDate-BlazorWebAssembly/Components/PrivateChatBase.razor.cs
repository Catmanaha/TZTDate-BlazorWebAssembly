using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDate_BlazorWebAssembly.Models.Chat;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class PrivateChatBase : ComponentBase
{
    private HubConnection hubConnection;
    [Inject]
    private IWebApiService webApiService { get; set; }
    [Parameter] public int CompanionId { get; set; }
    [Parameter] public int CurrentUserId { get; set; }
    public PrivateChat CurrentPrivateChat { get; set; }
    public string CurrentUserName { get; set; }
    public List<Message> Messages { get; set; }
    public CompanionsDto ChatDto { get; set; }
    public string newMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/chat")
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
    {
    var newMessage = new Message { Owner = user, Content = message, PrivateChatId = CurrentPrivateChat.Id };
    Messages.Add(newMessage);

    InvokeAsync(StateHasChanged);
    });

        await hubConnection.StartAsync();
        ChatDto = await GetChatData();
        CurrentPrivateChat = ChatDto.PrivateChat;
        CurrentUserName = ChatDto?.CurrentUser?.Username ?? throw new ArgumentNullException();
        Messages = CurrentPrivateChat.Messages ?? new List<Message>();
        await hubConnection.InvokeAsync("JoinGroup", CurrentPrivateChat.PrivateChatHashName);
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }

    private async Task<CompanionsDto> GetChatData()
    {
        return await webApiService.GoToChat(this.CurrentUserId, this.CompanionId);
    }

    protected async Task SendMessage()
    {
        await SendMessageToServer(newMessage);
        newMessage = string.Empty;
    }

    async Task<string> GetConnectionIdAsync() => await hubConnection.InvokeAsync<string>("GetConnectionId");

    async Task SendMessageToServer(string message)
    {
        await hubConnection.InvokeAsync("SendMessageToGroup", CurrentUserName, CurrentPrivateChat.PrivateChatHashName, message);
    }
}