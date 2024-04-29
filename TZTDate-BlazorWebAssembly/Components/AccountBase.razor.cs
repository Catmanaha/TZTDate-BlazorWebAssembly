using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class AccountBase : ComponentBase
{
    [Inject]
    private IWebApiService webApiService { get; set; }
    public AccountDto? accountDto { get; set; }
    public string? ErrorText { get; set; }
    protected override async Task OnInitializedAsync()
    {
        string myId = await webApiService.GetUserIdFromJwt();

        string json = await GetAccountDataAsync(myId);

        accountDto = JsonConvert.DeserializeObject<AccountDto>(json);
    }

    public async Task<string> GetAccountDataAsync(string id)
    {
        return await webApiService.GetAccountData(int.Parse(id));
    }

    public async Task<string> ChangeUsernameAsync()
    {
        string message = await webApiService.UpdateUsernameAsync(accountDto.User.Username, accountDto.User.Description);

        if (message is null) {
            return null;
        }
        
        ErrorText = message;

        return message;
    }
}
