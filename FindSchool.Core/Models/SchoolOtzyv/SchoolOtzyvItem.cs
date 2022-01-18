using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models.SchoolOtzyv;

[Table("schoolotzyv")]
public class SchoolOtzyvItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [Column("rating")]
    public decimal Rating { get; set; }

    [Column("comment_count")]
    public int CommentCount { get; set; }
}