using Microsoft.AspNetCore.Components.Authorization;
using TZTDateBlazorWebAssembly.DelegateHandlers;
using TZTDateBlazorWebAssembly.Providers;
using TZTDateBlazorWebAssembly.Services;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Extensions;

public static class DependencyInjections
{
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
        services.AddScoped<IIpifyApiService, IpifyApiService>();
        services.AddTransient<AuthorizationHandler>();
    }
}
