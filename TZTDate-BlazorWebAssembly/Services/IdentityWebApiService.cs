using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Responses;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class IdentityWebApiService : IIdentityService
{
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly IIpifyApiService ipifyApiService;

    public IdentityWebApiService(
        HttpClient httpClient,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider,
        IIpifyApiService ipifyApiService
    )
    {
        this.httpClient = httpClient;
        this.localStorageService = localStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
        this.ipifyApiService = ipifyApiService;
    }

    public async Task<string> Login(UserLoginDto loginDto)
    {
        loginDto.IpAddress = await ipifyApiService.GetCurrentIpAddress();

        var loginResponse = await httpClient.PostAsJsonAsync("Auth/Login", loginDto);

        if (loginResponse.IsSuccessStatusCode && loginResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var response = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            await localStorageService.SetItemAsStringAsync("jwt", response.AccessToken);
            await localStorageService.SetItemAsStringAsync("refreshToken", response.RefreshToken.ToString());

            await authenticationStateProvider.GetAuthenticationStateAsync();

            return null;
        }
        else
        {
            var error = await loginResponse.Content.ReadAsStringAsync();
            return error;
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
}
