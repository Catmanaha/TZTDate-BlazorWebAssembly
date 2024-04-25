using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class IpifyApiService : IIpifyApiService
{
    public IpifyApiService(ICorsApiService corsApiService)
    {
        CorsApiService = corsApiService;
    }

    public ICorsApiService CorsApiService { get; }

    public async Task<string> GetCurrentIpAddress(string format = "text")
    {
        return await CorsApiService.GetWithCorsAsync($"http://api.ipify.org/?format={format}");
        
    }
    
}
