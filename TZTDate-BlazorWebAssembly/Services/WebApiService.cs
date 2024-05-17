using System.Net.Http.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class WebApiService : IWebApiService
{
    private readonly HttpClient httpClient;

    public WebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CompanionsDto> GoToChat(int currentUserId, int companionId)
    {
        var response = await this.httpClient.PostAsJsonAsync<CompanionsDto>($"Chat/PrivateChat?companionId={companionId}&currentUserId={currentUserId}", null);
        return await response.Content.ReadFromJsonAsync<CompanionsDto>() ?? new CompanionsDto();
    }

}
