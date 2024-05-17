using Microsoft.AspNetCore.Components;
using TZTDate_BlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class IndexBase : ComponentBase
{
    [Inject]
    private IUserWebApiService userWebApiService { get; set; }
    public User? Me { get; set; }
    public IEnumerable<User>? RecomendationUsers { get; set; }
    public async Task GetRecomendations()
    {
        var recomendations = await userWebApiService.GetRecomendationsAsync();
        RecomendationUsers = recomendations.RecomendationUsers ?? new List<User>();
        Me = recomendations.Me;
    }

    protected override async Task OnInitializedAsync()
    {
        await GetRecomendations();
    }

}