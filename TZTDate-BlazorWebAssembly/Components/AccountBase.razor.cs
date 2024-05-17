using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class AccountBase : ComponentBase
{
    [Inject]
    private IIdentityService identityService { get; set; }
    [Inject]
    private IUserWebApiService userWebApiService { get; set; }
    public AccountDto? accountDto { get; set; }
    public string? ErrorText { get; set; }
    public string? MyInterests { get; set; }
    protected override async Task OnInitializedAsync()
    {
        string myId = await identityService.GetUserIdFromJwt();

        string json = await GetAccountDataAsync(myId);

        accountDto = JsonConvert.DeserializeObject<AccountDto>(json);
    }

    public async Task<string> GetAccountDataAsync(string id)
    {
        return await userWebApiService.GetAccountData(int.Parse(id));
    }

    public async Task<string> ChangeUsernameAsync()
    {
        string message = await userWebApiService.UpdateUsernameAsync(accountDto.User.Username, accountDto.User.Description);

        if (message is null) {
            return null;
        }
        
        ErrorText = message;

        return message;
    }
}
