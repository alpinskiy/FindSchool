using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.Google;

public struct LatLng
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lon { get; set; }
}