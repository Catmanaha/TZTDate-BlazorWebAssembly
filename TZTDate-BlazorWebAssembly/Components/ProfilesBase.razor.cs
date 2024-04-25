using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class ProfilesBase : ComponentBase
{
    public string? SearchingByName { get; set; }
    public int? SearchingStartAge { get; set; }
    public int? SearchingEndAge { get; set; }
    public string? SearchingInterests { get; set; }
    public string? SearchingGender { get; set; }

    public ProfilesDto? profiles { get; set; }

    [Inject]
    private IWebApiService webApiService { get; set; }
    [Inject]
    private ILocalStorageService localStorageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        profiles = await GetProfiles();

        SearchingStartAge = profiles.searchingStartAge;
        SearchingEndAge = profiles.searchingEndAge;
        SearchingGender = profiles.searchingGender;
    }

    private async Task<ProfilesDto> GetProfiles()
    {
        var id = await GetUserIdFromJwtAsync();
        return await webApiService.GetProfiles(id, SearchingByName, SearchingStartAge, SearchingEndAge, SearchingInterests, SearchingGender);
    }

    private async Task<string> GetUserIdFromJwtAsync()
    {
        string jwtToken = await localStorageService.GetItemAsStringAsync("jwt");
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(jwtToken);

        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim != null)
        {
            return userIdClaim.Value.ToString();
        }

        return null;
    }

    public async Task FilterSearch()
    {
        profiles = await GetProfiles();
    }
}
