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
    private readonly List<string> messages = new();
    [Inject]
    private IWebApiService webApiService { get; set; }
    
    public IJSRuntime JSRuntime { get; set; }
    [Parameter] public int CompanionId { get; set; }
    [Parameter] public int CurrentUserId { get; set; }
    public PrivateChat CurrentPrivateChat { get; set; }
    public string CurrentUserName { get; set; }
    public List<Message> Messages { get; set; }
    public CompanionsDto ChatDto { get; set; }
    public string ConnectionId { get; set; }
    public string newMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/chat")
            .Build();
        await hubConnection.StartAsync();
        ChatDto = await GetChatData();
        CurrentPrivateChat = ChatDto.PrivateChat;
        CurrentUserName = ChatDto?.CurrentUser?.Username ?? throw new ArgumentNullException();
        Messages = CurrentPrivateChat.Messages ?? new List<Message>();
        ConnectionId = await GetConnectionIdAsync();
        await hubConnection.InvokeAsync("JoinGroup", CurrentPrivateChat.PrivateChatHashName);
    }

    public async ValueTask DisposeAsync()
    {
        if(hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }

    private async Task<CompanionsDto> GetChatData()
    {
        return await webApiService.GoToChat(this.CurrentUserId, this.CompanionId);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("interop.setupSignalR");
        }
    }

    protected async Task SendMessage()
    {
        await SendMessageToServer(newMessage);
        newMessage = string.Empty;
    }

    async Task<string> GetConnectionIdAsync() => await hubConnection.InvokeAsync<string>("GetConnectionId");

    async Task SendMessageToServer(string message)
    {
        await hubConnection.InvokeAsync("SendMessageToGroup",  CurrentUserName,  CurrentPrivateChat.PrivateChatHashName,  message);
    }
}