using TZTDateBlazorWebAssembly.Services;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Extensions;

public static class HttpClients
{
    public static void InjectHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICountryApiService, CountryApiService>(o =>
        {
            o.BaseAddress = new Uri(configuration["CountryApi:Url"]);
            o.DefaultRequestHeaders.Add("X-CSCAPI-KEY", configuration["CountryApi:Key"]);
        });

        services.AddHttpClient<IWebApiService, WebApiService>(o =>
        {
            o.BaseAddress = new Uri(configuration["WebApi:Url"]);
        });
        
        services.AddHttpClient<ICorsApiService, CorsApiService>(o =>
        {
            o.BaseAddress = new Uri(configuration["CorsApi:Url"]);
            o.DefaultRequestHeaders.Add("origin", configuration["CorsApi:Origin"]);
        });
    }
}
