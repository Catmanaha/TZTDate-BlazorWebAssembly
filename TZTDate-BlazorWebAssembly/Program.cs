using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TZTDate_BlazorWebAssembly;
using TZTDateBlazorWebAssembly.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.InjectHttpClients(builder.Configuration);
builder.Services.Inject();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
