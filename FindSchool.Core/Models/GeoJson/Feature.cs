using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.GeoJson;

public class Feature
{
    [JsonPropertyName("type")]
    public string Type => "Feature";

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, string?> Properties { get; set; } = new();

    [JsonPropertyName("otions")]
    public Dictionary<string, string?> Options { get; set; } = new();
}