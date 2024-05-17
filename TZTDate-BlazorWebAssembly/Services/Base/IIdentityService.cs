using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TZTDateBlazorWebAssembly.Dtos;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface IIdentityService
{
    public Task<string> Login(UserLoginDto loginDto);
    public Task Register(UserRegisterDto userRegisterDto);
    public Task<string> GetUserIdFromJwt();
}
