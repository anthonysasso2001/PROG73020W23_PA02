﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieProductionApp.Entities;

#nullable disable

namespace MovieProductionApp.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    partial class MovieDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieProductionApp.Entities.Actor", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActorId"));

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActorId");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            ActorId = 1,
                            FirstName = "Humphrey",
                            LastName = "Bogart"
                        },
                        new
                        {
                            ActorId = 2,
                            FirstName = "Ingrid",
                            LastName = "Bergman"
                        },
                        new
                        {
                            ActorId = 3,
                            FirstName = "Keanu",
                            LastName = "Reeves"
                        },
                        new
                        {
                            ActorId = 4,
                            FirstName = "Carrie-Anne",
                            LastName = "Moss"
                        },
                        new
                        {
                            ActorId = 5,
                            FirstName = "John",
                            LastName = "Travolta"
                        },
                        new
                        {
                            ActorId = 6,
                            FirstName = "Uma",
                            LastName = "Thurman"
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Casting", b =>
                {
                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActorId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("Castings");

                    b.HasData(
                        new
                        {
                            ActorId = 1,
                            MovieId = 1,
                            Role = "Rick Blaine"
                        },
                        new
                        {
                            ActorId = 2,
                            MovieId = 1,
                            Role = "Ilsa Lund"
                        },
                        new
                        {
                            ActorId = 3,
                            MovieId = 2,
                            Role = "Neo"
                        },
                        new
                        {
                            ActorId = 4,
                            MovieId = 2,
                            Role = "Trinity"
                        },
                        new
                        {
                            ActorId = 5,
                            MovieId = 3,
                            Role = "Vincet Vega"
                        },
                        new
                        {
                            ActorId = 6,
                            MovieId = 3,
                            Role = "Mia Wallace"
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Genre", b =>
                {
                    b.Property<string>("GenreId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            GenreId = "A",
                            Name = "Action"
                        },
                        new
                        {
                            GenreId = "C",
                            Name = "Comedy"
                        },
                        new
                        {
                            GenreId = "D",
                            Name = "Drama"
                        },
                        new
                        {
                            GenreId = "H",
                            Name = "Horror"
                        },
                        new
                        {
                            GenreId = "M",
                            Name = "Musical"
                        },
                        new
                        {
                            GenreId = "R",
                            Name = "RomCom"
                        },
                        new
                        {
                            GenreId = "S",
                            Name = "SciFi"
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovieId"));

                    b.Property<string>("GenreId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("MovieId");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            MovieId = 1,
                            GenreId = "D",
                            Name = "Casablanca",
                            Year = 1942
                        },
                        new
                        {
                            MovieId = 2,
                            GenreId = "A",
                            Name = "The Matrix",
                            Year = 1998
                        },
                        new
                        {
                            MovieId = 3,
                            GenreId = "C",
                            Name = "Pulp Fiction",
                            Year = 1992
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.MovieApiData", b =>
                {
                    b.Property<int?>("MovieId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductionStudioId")
                        .HasColumnType("int");

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<int?>("StreamCompanyInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfOffer")
                        .HasColumnType("datetime2");

                    b.HasKey("MovieId", "ProductionStudioId");

                    b.HasIndex("ProductionStudioId");

                    b.HasIndex("StreamCompanyInfoId");

                    b.ToTable("MovieApiData");

                    b.HasData(
                        new
                        {
                            MovieId = 1,
                            ProductionStudioId = 1,
                            Availability = true,
                            TimeOfOffer = new DateTime(2010, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            MovieId = 2,
                            ProductionStudioId = 1,
                            Availability = false,
                            TimeOfOffer = new DateTime(2012, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            MovieId = 3,
                            ProductionStudioId = 1,
                            Availability = true,
                            TimeOfOffer = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.ProductionStudio", b =>
                {
                    b.Property<int>("ProductionStudioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductionStudioId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductionStudioId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProductionStudios");

                    b.HasData(
                        new
                        {
                            ProductionStudioId = 1,
                            Name = "MPC Ltd."
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("MovieId");

                    b.ToTable("Review");

                    b.HasData(
                        new
                        {
                            ReviewId = 1,
                            Comments = "A classic!",
                            MovieId = 1,
                            Rating = 5
                        },
                        new
                        {
                            ReviewId = 2,
                            Comments = "They should have gotten together in the end!",
                            MovieId = 1,
                            Rating = 3
                        },
                        new
                        {
                            ReviewId = 3,
                            Comments = "Too slow of a pace",
                            MovieId = 1,
                            Rating = 3
                        },
                        new
                        {
                            ReviewId = 4,
                            Comments = "Based on Descarte's \"brain in a vat\" thought experiment",
                            MovieId = 2,
                            Rating = 4
                        },
                        new
                        {
                            ReviewId = 5,
                            Comments = "Very philosophical",
                            MovieId = 2,
                            Rating = 3
                        },
                        new
                        {
                            ReviewId = 6,
                            Comments = "Very violent but also very funny and clever",
                            MovieId = 3,
                            Rating = 5
                        });
                });

            modelBuilder.Entity("MovieProductionApp.Entities.StreamCompanyInfo", b =>
                {
                    b.Property<int?>("StreamCompanyInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("StreamCompanyInfoId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("StreamGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("challengeUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("verificationStatus")
                        .HasColumnType("bit");

                    b.Property<string>("webApiUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StreamCompanyInfoId");

                    b.HasAlternateKey("Name", "webApiUrl");

                    b.HasIndex("StreamGUID")
                        .IsUnique();

                    b.ToTable("StreamCompanies");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Casting", b =>
                {
                    b.HasOne("MovieProductionApp.Entities.Actor", "Actor")
                        .WithMany("Castings")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieProductionApp.Entities.Movie", "Movie")
                        .WithMany("Castings")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Movie", b =>
                {
                    b.HasOne("MovieProductionApp.Entities.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.MovieApiData", b =>
                {
                    b.HasOne("MovieProductionApp.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieProductionApp.Entities.ProductionStudio", "ProductionStudio")
                        .WithMany()
                        .HasForeignKey("ProductionStudioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieProductionApp.Entities.StreamCompanyInfo", "StreamPartner")
                        .WithMany("RegisteredMovies")
                        .HasForeignKey("StreamCompanyInfoId");

                    b.Navigation("Movie");

                    b.Navigation("ProductionStudio");

                    b.Navigation("StreamPartner");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Review", b =>
                {
                    b.HasOne("MovieProductionApp.Entities.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Actor", b =>
                {
                    b.Navigation("Castings");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.Movie", b =>
                {
                    b.Navigation("Castings");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("MovieProductionApp.Entities.StreamCompanyInfo", b =>
                {
                    b.Navigation("RegisteredMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
