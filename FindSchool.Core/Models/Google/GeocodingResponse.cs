using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.Google;

public struct GeocodingResponse
{
    [JsonPropertyName("results")]
    public List<GeocodingResult>? Results { get; set; }
}