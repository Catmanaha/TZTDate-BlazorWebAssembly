namespace TZTDateBlazorWebAssembly.Responses;

public class UpdateTokenResponse
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
}
