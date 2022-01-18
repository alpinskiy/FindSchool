using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models.Esir;

[Table("esir")]
public sealed class EsirInfo
{
    public EsirInfo(EsirCategory category, string url)
    {
        Category = category;
        Url = url;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("category")]
    public EsirCategory Category { get; set; }

    [Column("url")]
    public string Url { get; set; }

    [Column("url_text")]
    public string? UrlText { get; set; }
}