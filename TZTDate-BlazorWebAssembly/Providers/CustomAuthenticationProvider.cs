using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using TZTDateBlazorWebAssembly.Requests;
using TZTDateBlazorWebAssembly.Responses;

namespace TZTDateBlazorWebAssembly.Providers;

public class CustomAuthenticationProvider : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler jwtTokenHandler;
    private readonly ILocalStorageService localStorageService;

    public CustomAuthenticationProvider(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
        this.jwtTokenHandler = new JwtSecurityTokenHandler();
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwt = await this.localStorageService.GetItemAsStringAsync("jwt");
        var refreshToken = await this.localStorageService.GetItemAsStringAsync("refreshToken");
        Guid.TryParse(refreshToken, out Guid refreshTokenGuid);
        var claimsIdentity = await this.GetClaimsIdentityAsync(jwt, refreshTokenGuid);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationState = new AuthenticationState(claimsPrincipal);

        base.NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

        return authenticationState;
    }

    private async Task<ClaimsIdentity> GetClaimsIdentityAsync(string? jwt, Guid? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(jwt) || refreshToken is null)
        {
            return new ClaimsIdentity();
        }

        var validationResult = await jwtTokenHandler.ValidateTokenAsync(
            jwt,
            new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = "TZTDate",

                ValidateIssuer = true,
                ValidIssuers = new List<string>() { "Timur", "Zabil", "Tamerlan" },

                SignatureValidator = (token, validationParameters) => new JwtSecurityToken(token),

                ValidateLifetime = true,
                RequireExpirationTime = true,
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires > DateTime.UtcNow,
            }
        );

        if (validationResult.IsValid == false)
        {
            if (validationResult.Exception is SecurityTokenInvalidLifetimeException lifetimeException)
            {
                var httpClient = new HttpClient();

                var updateTokenResponse = await httpClient.PutAsJsonAsync(
                    "http://localhost:5000/api/Auth/UpdateToken",

                       new UpdateTokenRequest
                       {
                           AccessToken = jwt,
                           RefreshToken = refreshToken,
                           IpAddress = await GetIpAddress()
                       }

                    );

                if (updateTokenResponse.IsSuccessStatusCode && updateTokenResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await updateTokenResponse.Content.ReadFromJsonAsync<UpdateTokenResponse>();

                    await this.localStorageService.SetItemAsStringAsync("jwt", response.AccessToken);
                    await this.localStorageService.SetItemAsStringAsync("refreshToken", response.RefreshToken.ToString());

                    var newToken = jwtTokenHandler.ReadJwtToken(response.AccessToken);

                    return new ClaimsIdentity(newToken.Claims, "jwt");
                }
            }

            return new ClaimsIdentity();
        }

        var token = jwtTokenHandler.ReadJwtToken(jwt);

        return new ClaimsIdentity(token.Claims, "jwt");
    }

    public async Task<string> GetIpAddress()
    {
        return await GetWithCorsAsync("https://cors-anywhere.herokuapp.com/http://api.ipify.org/?format=text");
    }
    
    public async Task<string> GetWithCorsAsync(string url)
    {
        var corsApiHost = "cors-anywhere.herokuapp.com";
        var corsApiUrl = "https://" + corsApiHost + "/";
        var origin = "www.flirtify.tech";

        var uri = new Uri(url);
        if (uri.Host != origin && uri.Host != corsApiHost)
        {
            url = corsApiUrl + url;
        }

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("origin", origin);
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return content;
    }
}
