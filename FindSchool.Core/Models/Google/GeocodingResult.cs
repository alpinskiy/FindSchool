using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.Google;

public class GeocodingResult
{
    //[JsonPropertyName("address_components")]
    //public AddressComponent AddressComponents { get; set; }

    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }

    [JsonPropertyName("geometry")]
    public AddressGeometry Geometry { get; set; }

    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }

    [JsonPropertyName("place_id")]
    public string? PlaceId { get; set; }
}