using System.Text.Json.Serialization;
using TZTDate_BlazorWebAssembly.Models;

namespace TZTDate_BlazorWebAssembly.Dtos;

public class UserDetailsDto
{
    [JsonPropertyName("myUser")]
    public User CurrentUser { get; set; }
    [JsonPropertyName("user")]
    public User ViewedUser { get; set; }
    [JsonPropertyName("imageUris")]
    public List<string> ImageUris { get; set; }
    [JsonPropertyName("interests")]
    public string Interests { get; set; }

}