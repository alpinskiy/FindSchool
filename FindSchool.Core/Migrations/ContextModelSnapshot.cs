// <auto-generated />
using System;
using FindSchool.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FindSchool.Core.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("FindSchool.Core.Models.APeterburg.APeterburgDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<string>("Contacts")
                        .HasColumnType("TEXT")
                        .HasColumnName("contacts");

                    b.Property<string>("Director")
                        .HasColumnType("TEXT")
                        .HasColumnName("director");

                    b.Property<string>("District")
                        .HasColumnType("TEXT")
                        .HasColumnName("district");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("ForeignLanguages")
                        .HasColumnType("TEXT")
                        .HasColumnName("foreign_languages");

                    b.Property<string>("FreeText")
                        .HasColumnType("TEXT")
                        .HasColumnName("free_text");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT")
                        .HasColumnName("full_name");

                    b.Property<string>("Kind")
                        .HasColumnType("TEXT")
                        .HasColumnName("kind");

                    b.Property<string>("ShortName")
                        .HasColumnType("TEXT")
                        .HasColumnName("short_name");

                    b.Property<string>("SubwayNearby")
                        .HasColumnType("TEXT")
                        .HasColumnName("subway_nearby");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("apeterburg_details");
                });

            modelBuilder.Entity("FindSchool.Core.Models.APeterburg.APeterburgItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<int>("CommentCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("comment_count");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<decimal>("Rating")
                        .HasColumnType("TEXT")
                        .HasColumnName("rating");

                    b.Property<int>("ViewCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("view_count");

                    b.Property<int>("VoteCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("vote_count");

                    b.HasKey("Id");

                    b.ToTable("apeterburg");
                });

            modelBuilder.Entity("FindSchool.Core.Models.Esir.EsirInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER")
                        .HasColumnName("category");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("url");

                    b.Property<string>("UrlText")
                        .HasColumnType("TEXT")
                        .HasColumnName("url_text");

                    b.HasKey("Id");

                    b.ToTable("esir");
                });

            modelBuilder.Entity("FindSchool.Core.Models.Geocoding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("APeterburgId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("apeterburg_id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("response");

                    b.Property<int?>("SchoolOtzyvId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("schoolotzyv_id");

                    b.HasKey("Id");

                    b.ToTable("geocoding");
                });

            modelBuilder.Entity("FindSchool.Core.Models.SchoolOtzyv.SchoolOtzyvDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.HasKey("Id");

                    b.ToTable("schoolotzyv_details");
                });

            modelBuilder.Entity("FindSchool.Core.Models.SchoolOtzyv.SchoolOtzyvItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CommentCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("comment_count");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<decimal>("Rating")
                        .HasColumnType("TEXT")
                        .HasColumnName("rating");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("schoolotzyv");
                });
#pragma warning restore 612, 618
        }
    }
}
