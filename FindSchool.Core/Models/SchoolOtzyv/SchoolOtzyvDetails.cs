using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models.SchoolOtzyv;

[Table("schoolotzyv_details")]
public class SchoolOtzyvDetails
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("address")]
    public string? Address { get; set; }
}