using TZTDateBlazorWebAssembly.Enums;

namespace TZTDate_BlazorWebAssembly.Models;

public class User

{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime BirthDateTime { get; set; }

    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - BirthDateTime.Year;
            if (BirthDateTime.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
    }

    public Gender Gender { get; set; }
    public Address? Address { get; set; }
    public string? Description { get; set; }
    public string[]? ProfilePicPaths { get; set; }
    public DateTime CreatedAt { get; set; }

    public Gender? SearchingGender { get; set; }
    public int SearchingAgeStart { get; set; }
    public int SearchingAgeEnd { get; set; }
    public string? Interests { get; set; }

    public List<User>? Followers { get; set; }
    public List<User>? Followed { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}