using System.Net.Http.Json;
using Newtonsoft.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Enums;
using TZTDateBlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class UserWebApiService : IUserWebApiService
{
    private readonly HttpClient httpClient;
    private readonly IWebApiService webApiService;
    private readonly IIdentityService identityService;

    public UserWebApiService(HttpClient httpClient, IIdentityService identityService)
    {
        this.identityService = identityService;
        this.httpClient = httpClient;
        this.webApiService = webApiService;
    }

    public async Task SetMembership(int currentUserId, int viewedUserId)
    {
        await this.httpClient.PostAsync($"User/MembershipAction?currentUserId={currentUserId}&userToActionId={viewedUserId}", null);
    }

    public async Task<UserDetailsDto> GetDetails(int currentUserId, int viewedUserId)
    {
        var userDetailsDto = await this.httpClient.GetFromJsonAsync<UserDetailsDto>($"User/Details?currentUserId={currentUserId}&viewedUserId={viewedUserId}");
        return userDetailsDto ?? new UserDetailsDto();
    }

    public async Task<string> GetAccountData(int userId)
    {
        return await httpClient.GetStringAsync($"User/Account?id={userId}");
    }

    public async Task<string> UpdateUsernameAsync(string newUsername, string newDescription)
    {
        string id = await identityService.GetUserIdFromJwt();

        string url = $"User/ChangeUsername";

        UpdateUsernameDto updateUsernameDto = new UpdateUsernameDto()
        {
            Id = id,
            NewUsername = newUsername,
            NewDescription = newDescription
        };

        var responce = await httpClient.PutAsJsonAsync(url, updateUsernameDto);

        if (responce.IsSuccessStatusCode)
            return null;

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<DateUserAndRecomendations>? GetRecomendationsAsync()
    {
        string id = await identityService.GetUserIdFromJwt();

        var response = await httpClient.GetFromJsonAsync<DateUserAndRecomendations>($"Home/Index?userId={id}");

        return response;
    }
    public async Task<ProfilesDto> GetProfiles(string userId, string searchByName, int? startAge, int? endAge, string interests, Gender searchGender)
    {

        var url =
        $"User/Profiles?userId={userId}&searchByName={searchByName}&startAge={startAge}&endAge={endAge}&interests={interests}&searchGender={searchGender}";

        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync();

        ProfilesDto profiles = JsonConvert.DeserializeObject<ProfilesDto>(json);

        return profiles;
    }
}
