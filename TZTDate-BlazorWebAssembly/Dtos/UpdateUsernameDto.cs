namespace TZTDate_BlazorWebAssembly.Dtos;

public class UpdateUsernameDto
{
    public string Id { get; set; }
    public string? NewUsername { get; set; }
    public string? NewDescription { get; set; }
}