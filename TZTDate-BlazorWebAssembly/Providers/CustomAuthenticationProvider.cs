using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
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

        var claimsIdentity = await this.GetClaimsIdentityAsync(jwt, Guid.Parse(refreshToken));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationState = new AuthenticationState(claimsPrincipal);

        base.NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

        return authenticationState;
    }

    private async Task<ClaimsIdentity> GetClaimsIdentityAsync(string? jwt, Guid? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(jwt) || refreshToken == null)
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
                    new
                    {
                        AccessToken = jwt,
                        RefreshToken = refreshToken
                    });

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
}
