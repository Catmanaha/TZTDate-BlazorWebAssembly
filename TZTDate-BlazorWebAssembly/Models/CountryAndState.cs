using System.Text.Json.Serialization;

namespace TZTDateBlazorWebAssembly.Models;

public class CountryAndState
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iso2")]
    public string Iso2 { get; set; }
}
