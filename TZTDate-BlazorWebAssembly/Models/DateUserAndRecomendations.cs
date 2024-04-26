using TZTDate_BlazorWebAssembly.Models;

namespace TZTDateBlazorWebAssembly.Models;

public class DateUserAndRecomendations
{
    public User? Me { get; set; }
    public IEnumerable<User>? RecomendationUsers { get; set; }
}