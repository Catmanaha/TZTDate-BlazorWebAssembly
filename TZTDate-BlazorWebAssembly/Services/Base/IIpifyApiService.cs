namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IIpifyApiService
{
    public Task<string> GetCurrentIpAddress(string format = "text");
}
