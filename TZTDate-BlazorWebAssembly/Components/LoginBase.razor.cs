using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public class LoginBase : ComponentBase
{
    [Inject]
    private NavigationManager navigationManager { get; set; }

    [Inject]
    private IWebApiService webApiService { get; set; }

    public UserLoginDto loginDto { get; set; } = new();
    public string? ErrorText { get; set; }

    public async Task OnLoginFormSubmitAsync(EditContext editContext)
    {
        var text = await webApiService.Login(loginDto);

        if (text == null)
        {
            navigationManager.NavigateTo("/");
            return;
        }

        ErrorText = text;
    }
}
