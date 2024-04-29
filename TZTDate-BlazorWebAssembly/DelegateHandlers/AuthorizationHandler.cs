using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace TZTDateBlazorWebAssembly.DelegateHandlers;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public AuthorizationHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _localStorageService.GetItemAsStringAsync("jwt");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}