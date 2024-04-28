using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TZTDate_BlazorWebAssembly;
using TZTDateBlazorWebAssembly.Extensions;
using TZTDateBlazorWebAssembly.Models.Zodiac.Entities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();

builder.Services.InjectHttpClients(builder.Configuration, builder.HostEnvironment);
builder.Services.Inject();

builder.Services.AddAuthorizationCore();
var host = builder.Build();


ZodiacSign.httpClient = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient("ZodiacSign");

await host.RunAsync();