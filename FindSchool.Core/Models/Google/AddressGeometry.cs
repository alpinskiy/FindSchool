using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.Google;

public struct AddressGeometry
{
    [JsonPropertyName("location")]
    public LatLng Location { get; set; }

    [JsonPropertyName("location_type")]
    public string LocationType { get; set; }

    [JsonPropertyName("bounds")]
    public LatLngBounds Bounds { get; set; }

    [JsonPropertyName("viewport")]
    public LatLngBounds Viewport { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }
}