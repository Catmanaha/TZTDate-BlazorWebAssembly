using TZTDateBlazorWebAssembly.Dtos;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IWebApiService
{
    public Task Register(UserRegisterDto userRegisterDto);
    public Task Login(UserLoginDto loginDto);
}
