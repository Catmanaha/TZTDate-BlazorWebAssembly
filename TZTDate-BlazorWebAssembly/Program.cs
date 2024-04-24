using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tewr.Blazor.FileReader;
using TZTDate_BlazorWebAssembly;
using TZTDateBlazorWebAssembly.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(
        "developer",
        builder => builder.RequireRole("developer", "tech leader")
        );
});

await builder.Build().RunAsync();
