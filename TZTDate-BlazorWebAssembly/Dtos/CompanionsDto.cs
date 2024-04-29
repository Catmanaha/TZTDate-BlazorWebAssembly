using TZTDate_BlazorWebAssembly.Models;
using TZTDate_BlazorWebAssembly.Models.Chat;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class CompanionsDto
{
    public User? CurrentUser { get; set; }
    public PrivateChat PrivateChat { get; set; }
}