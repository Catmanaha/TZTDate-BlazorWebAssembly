using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class CorsApiService : ICorsApiService
{
    private readonly HttpClient httpClient;

    public CorsApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> GetWithCorsAsync(string url)
    {
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}
