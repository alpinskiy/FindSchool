using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.GeoJson;

public struct FeatureCollection
{
    [JsonPropertyName("type")]
    public string Type => "FeatureCollection";

    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; } = new();
}