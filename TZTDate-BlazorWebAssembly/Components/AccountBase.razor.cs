using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class AccountBase : ComponentBase
{
    [Inject]
    private IWebApiService webApiService { get; set; }
    public string? myId = string.Empty;
    public string? text = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        myId = await webApiService.GetUserIdFromJwt();

        string json = await GetAccountDataAsync(myId);

        AccountDto accountDto = JsonConvert.DeserializeObject<AccountDto>(json);
    }

    public async Task<string> GetAccountDataAsync(string id)
    {
        return await webApiService.GetAccountData(int.Parse(id));
    }
}
