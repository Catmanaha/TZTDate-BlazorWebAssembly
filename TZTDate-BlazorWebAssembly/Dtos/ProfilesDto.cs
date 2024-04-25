using TZTDate_BlazorWebAssembly.Models;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class ProfilesDto
{
    public int searchingStartAge { get; set; }
    public int searchingEndAge { get; set; }
    public string searchingGender { get; set; }
    public List<User>? users { get; set; }
}