using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Responses;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class WebApiService : IWebApiService
{
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly IIpifyApiService ipifyApiService;

    public WebApiService(HttpClient httpClient,
    ILocalStorageService localStorageService,
    AuthenticationStateProvider authenticationStateProvider,
    IIpifyApiService ipifyApiService)
    {
        this.localStorageService = localStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
        this.ipifyApiService = ipifyApiService;
        this.httpClient = httpClient;
    }

    public async Task<string> GetAccountData(int userId)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await localStorageService.GetItemAsStringAsync("jwt"));
        return await httpClient.GetStringAsync($"User/Account?id={userId}");
    }

    public async Task<ProfilesDto> GetProfiles(string userId, string searchByName, int? startAge, int? endAge, string interests, string searchGender)
    {

        var url =
        $"User/Profiles?userId={userId}&searchByName={searchByName}&startAge={startAge}&endAge={endAge}&interests={interests}&searchGender={searchGender}";

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await localStorageService.GetItemAsStringAsync("jwt"));
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync();

        ProfilesDto profiles = JsonConvert.DeserializeObject<ProfilesDto>(json);

        return profiles;
    }

    public async Task<string> GetUserIdFromJwt()
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

    public async Task Login(UserLoginDto loginDto)
    {

        loginDto.IpAddress = await ipifyApiService.GetCurrentIpAddress();

        var loginResponse = await httpClient.PostAsJsonAsync("Auth/Login", loginDto);

        if (loginResponse.IsSuccessStatusCode && loginResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var response = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            await localStorageService.SetItemAsStringAsync("jwt", response.AccessToken);
            await localStorageService.SetItemAsStringAsync("refreshToken", response.RefreshToken.ToString());

            await authenticationStateProvider.GetAuthenticationStateAsync();
        }
        else
        {
            var error = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine(error);
        }
    }

    public async Task Register(UserRegisterDto userRegisterDto)
    {
        var content = new MultipartFormDataContent();
        var properties = userRegisterDto.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.Name.Contains("Image") && property.GetValue(userRegisterDto) is byte[])
            {
                var value = property.GetValue(userRegisterDto) as byte[];
                if (value != null)
                {
                    var base64Value = Convert.ToBase64String(value);
                    content.Add(new StringContent(base64Value), property.Name);
                }
            }
            else
            {
                var value = property.GetValue(userRegisterDto);
                if (value != null)
                {
                    content.Add(new StringContent(value.ToString()), property.Name);
                }
            }
        }

        await httpClient.PostAsync("Auth/Register", content);
    }
}
