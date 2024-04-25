using System.Text.Json.Serialization;

namespace TZTDateBlazorWebAssembly.Models;

public class City
{
    [JsonPropertyName("name")]
    public string Value { get; set; }
}
