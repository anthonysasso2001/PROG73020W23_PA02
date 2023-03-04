﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoviesApp.Entities;

#nullable disable

namespace MoviesApp.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    [Migration("20230304213314_assignInit")]
    partial class assignInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MoviesApp.Entities.Genre", b =>
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

            modelBuilder.Entity("MoviesApp.Entities.Movie", b =>
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

                    b.Property<int?>("ProductionStudioId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("Year")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("MovieId");

                    b.HasIndex("GenreId");

                    b.HasIndex("ProductionStudioId");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            MovieId = 1,
                            GenreId = "D",
                            Name = "Casablanca",
                            ProductionStudioId = 1,
                            Rating = 5,
                            Year = 1942
                        },
                        new
                        {
                            MovieId = 2,
                            GenreId = "A",
                            Name = "Apocalypse Now",
                            ProductionStudioId = 4,
                            Rating = 4,
                            Year = 1979
                        },
                        new
                        {
                            MovieId = 3,
                            GenreId = "C",
                            Name = "Annie Hall",
                            ProductionStudioId = 3,
                            Rating = 5,
                            Year = 1977
                        });
                });

            modelBuilder.Entity("MoviesApp.Entities.ProductionStudio", b =>
                {
                    b.Property<int>("ProductionStudioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductionStudioId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductionStudioId");

                    b.ToTable("Studios");

                    b.HasData(
                        new
                        {
                            ProductionStudioId = 1,
                            Name = "Warner Brothers"
                        },
                        new
                        {
                            ProductionStudioId = 2,
                            Name = "MPC Productions"
                        },
                        new
                        {
                            ProductionStudioId = 3,
                            Name = "Rollins and Joffe Productions"
                        },
                        new
                        {
                            ProductionStudioId = 4,
                            Name = "Omni Zoetrope"
                        });
                });

            modelBuilder.Entity("MoviesApp.Entities.Movie", b =>
                {
                    b.HasOne("MoviesApp.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoviesApp.Entities.ProductionStudio", "ProductionStudio")
                        .WithMany()
                        .HasForeignKey("ProductionStudioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("ProductionStudio");
                });
#pragma warning restore 612, 618
        }
    }
}
