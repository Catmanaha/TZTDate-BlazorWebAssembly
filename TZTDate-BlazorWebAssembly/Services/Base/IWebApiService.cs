using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Models;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IWebApiService
{
    public Task Register(UserRegisterDto userRegisterDto);
    public Task Login(UserLoginDto loginDto);
    public Task<ProfilesDto> GetProfiles(string userId, string searchByName, int? startAge, int? endAge, string interests, string searchGender);
    public Task<string> GetAccountData(int userId);
    public Task<string> GetUserIdFromJwt();
    public Task<DateUserAndRecomendations> GetRecomendationsAsync();
}
