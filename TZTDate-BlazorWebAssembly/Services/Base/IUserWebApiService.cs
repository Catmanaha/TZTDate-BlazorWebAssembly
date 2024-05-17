using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Enums;
using TZTDateBlazorWebAssembly.Models;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IUserWebApiService
{
    public Task SetMembership(int currentUserId, int viewedUserId);
    public Task<UserDetailsDto> GetDetails(int currentUserId, int viewedUserId);
    public Task<string> GetAccountData(int userId);
    public Task<string> UpdateUsernameAsync(string newUsername, string newDescription);
    public Task<DateUserAndRecomendations>? GetRecomendationsAsync();
    public Task<ProfilesDto> GetProfiles(string userId, string searchByName, int? startAge, int? endAge, string interests, Gender searchGender);
}
