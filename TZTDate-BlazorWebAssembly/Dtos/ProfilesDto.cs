using TZTDate_BlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Enums;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class ProfilesDto
{
    public int searchingStartAge { get; set; }
    public int searchingEndAge { get; set; }
    public Gender searchingGender { get; set; }
    public List<User>? users { get; set; }
}