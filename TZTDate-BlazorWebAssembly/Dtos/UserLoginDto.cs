using System.ComponentModel.DataAnnotations;

namespace TZTDateBlazorWebAssembly.Dtos;

public class UserLoginDto
{

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public string IpAddress { get; set; }
}