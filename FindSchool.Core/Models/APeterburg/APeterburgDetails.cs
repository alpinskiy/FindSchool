using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models.APeterburg;

[Table("apeterburg_details")]
public sealed class APeterburgDetails
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("full_name")]
    public string? FullName { get; set; }

    [Column("short_name")]
    public string? ShortName { get; set; }

    [Column("district")]
    public string? District { get; set; }

    [Column("kind")]
    public string? Kind { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("contacts")]
    public string? Contacts { get; set; }

    [Column("director")]
    public string? Director { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("subway_nearby")]
    public string? SubwayNearby { get; set; }

    [Column("foreign_languages")]
    public string? ForeignLanguages { get; set; }

    [Column("free_text")]
    public string? FreeText { get; set; }
}