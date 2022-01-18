using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindSchool.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apeterburg",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: true),
                    rating = table.Column<decimal>(type: "TEXT", nullable: false),
                    view_count = table.Column<int>(type: "INTEGER", nullable: false),
                    comment_count = table.Column<int>(type: "INTEGER", nullable: false),
                    vote_count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apeterburg", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "apeterburg_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    full_name = table.Column<string>(type: "TEXT", nullable: true),
                    short_name = table.Column<string>(type: "TEXT", nullable: true),
                    district = table.Column<string>(type: "TEXT", nullable: true),
                    kind = table.Column<string>(type: "TEXT", nullable: true),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    contacts = table.Column<string>(type: "TEXT", nullable: true),
                    director = table.Column<string>(type: "TEXT", nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: true),
                    subway_nearby = table.Column<string>(type: "TEXT", nullable: true),
                    foreign_languages = table.Column<string>(type: "TEXT", nullable: true),
                    free_text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apeterburg_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "esir",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category = table.Column<int>(type: "INTEGER", nullable: false),
                    url = table.Column<string>(type: "TEXT", nullable: false),
                    url_text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_esir", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "geocoding",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    apeterburg_id = table.Column<int>(type: "INTEGER", nullable: true),
                    schoolotzyv_id = table.Column<int>(type: "INTEGER", nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    response = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geocoding", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schoolotzyv",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    rating = table.Column<decimal>(type: "TEXT", nullable: false),
                    comment_count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schoolotzyv", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schoolotzyv_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    address = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schoolotzyv_details", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apeterburg");

            migrationBuilder.DropTable(
                name: "apeterburg_details");

            migrationBuilder.DropTable(
                name: "esir");

            migrationBuilder.DropTable(
                name: "geocoding");

            migrationBuilder.DropTable(
                name: "schoolotzyv");

            migrationBuilder.DropTable(
                name: "schoolotzyv_details");
        }
    }
}
