using System.ComponentModel.DataAnnotations;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public string IpAddress { get; set; }
}