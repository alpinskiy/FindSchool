using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.GeoJson;

public struct Geometry
{
    [JsonPropertyName("type")]
    public string Type => "Point";

    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; set; }
}