using System.Text.Json.Serialization;

namespace FindSchool.Core.Models.Google;

public struct AddressPlusCode
{
    [JsonPropertyName("global_code")]
    public string GlobalCode { get; set; }

    [JsonPropertyName("compound_code")]
    public string CompoundCode { get; set; }
}