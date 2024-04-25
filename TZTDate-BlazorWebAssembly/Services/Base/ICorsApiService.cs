namespace TZTDateBlazorWebAssembly.Services.Base;

public interface ICorsApiService
{
    public Task<string> GetWithCorsAsync(string url);
}
