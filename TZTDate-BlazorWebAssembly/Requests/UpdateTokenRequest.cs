namespace TZTDateBlazorWebAssembly.Requests;

public class UpdateTokenRequest
{
    public string AccessToken { get; set; }
    public Guid? RefreshToken { get; set; }
    public string IpAddress { get; set; }
}
