namespace CaelestiaStylixSync.Models;

public class CaelestiaScheme
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("flavour")]
    public string Flavour { get; set; } = "";

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "";

    [JsonPropertyName("variant")]
    public string Variant { get; set; } = "";

    [JsonPropertyName("colours")]
    public Dictionary<string, string> Colours { get; set; } = [];
}
