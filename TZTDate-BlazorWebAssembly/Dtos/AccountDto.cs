using TZTDate_BlazorWebAssembly.Models;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class AccountDto
{
    public User User { get; set; }
    public List<string> ImageUris { get; set; }
}