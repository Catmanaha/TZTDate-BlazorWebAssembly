using TZTDate_BlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Enums;
using TZTDateBlazorWebAssembly.Models;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IWebApiService
{
    public Task<CompanionsDto> GoToChat(int currentUserId, int companionId);
}
