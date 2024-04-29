using Microsoft.AspNetCore.Components;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Enums;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class ProfilesBase : ComponentBase
{
    public string? SearchingByName { get; set; }
    public int? SearchingStartAge { get; set; }
    public int? SearchingEndAge { get; set; }
    public string? SearchingInterests { get; set; }
    public Gender SearchingGender { get; set; }
    public string SearchingGenderString
    {
        get { return SearchingGender.ToString(); }
        set { SearchingGender = Enum.Parse<Gender>(value); }
    }

    public ProfilesDto? profiles { get; set; }

    [Inject]
    private IWebApiService webApiService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        profiles = await GetProfiles();

        SearchingStartAge = profiles.searchingStartAge;
        SearchingEndAge = profiles.searchingEndAge;
        SearchingGender = profiles.searchingGender;
    }

    private async Task<ProfilesDto> GetProfiles()
    {
        var id = await webApiService.GetUserIdFromJwt();
        return await webApiService.GetProfiles(id, SearchingByName, SearchingStartAge, SearchingEndAge, SearchingInterests, SearchingGender);
    }

    public async Task FilterSearch()
    {
        profiles = await GetProfiles();
    }

    public void SwitchGender()
    {
        System.Console.WriteLine(SearchingGender);
        SearchingGender = SearchingGender == Gender.Female ? Gender.Male : Gender.Female;
        SearchingGenderString = SearchingGender.ToString();
        StateHasChanged();
    }
}
