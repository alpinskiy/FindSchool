using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindSchool.Core.Models;

[Table("geocoding")]
public class Geocoding
{
    public Geocoding(string address, string response)
    {
        Address = address;
        Response = response;
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("apeterburg_id")]
    public int? APeterburgId { get; set; }

    [Column("schoolotzyv_id")]
    public int? SchoolOtzyvId { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("response")]
    public string Response { get; set; }
}