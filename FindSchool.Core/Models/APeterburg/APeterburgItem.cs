using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models.APeterburg;

[Table("apeterburg")]
public sealed class APeterburgItem
{
    public APeterburgItem(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("rating")]
    public decimal Rating { get; set; }

    [Column("view_count")]
    public int ViewCount { get; set; }

    [Column("comment_count")]
    public int CommentCount { get; set; }

    [Column("vote_count")]
    public int VoteCount { get; set; }
}