using FindSchool.Core.Models;
using FindSchool.Core.Models.APeterburg;
using FindSchool.Core.Models.Esir;
using FindSchool.Core.Models.SchoolOtzyv;
using Microsoft.EntityFrameworkCore;

namespace FindSchool.Core;

public class Context : DbContext
{
    public DbSet<EsirInfo> EsirSchools { get; set; } = null!;
    public DbSet<APeterburgItem> ApSchools { get; set; } = null!;
    public DbSet<APeterburgDetails> ApSchoolDetails { get; set; } = null!;
    public DbSet<Geocoding> Geocodings { get; set; } = null!;
    public DbSet<SchoolOtzyvItem> SchoolOtzyvItems { get; set; } = null!;
    public DbSet<SchoolOtzyvDetails> SchoolOtzyvDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var path = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "findschool.db");
        optionsBuilder.UseSqlite($"Data Source={path}");
    }
}