﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieInfoAPI.Model;

namespace MovieInfoAPI.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    partial class MovieDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CharacterMovie", b =>
                {
                    b.Property<Guid>("CharactersCharacterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MoviesMovieId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CharactersCharacterId", "MoviesMovieId");

                    b.HasIndex("MoviesMovieId");

                    b.ToTable("CharacterMovie");
                });

            modelBuilder.Entity("MovieInfoAPI.Models.Domain.Character", b =>
                {
                    b.Property<Guid>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharacterId");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("MovieInfoAPI.Models.Domain.Franchise", b =>
                {
                    b.Property<Guid>("FranchiseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("FranchiseId");

                    b.ToTable("Franchise");

                    b.HasData(
                        new
                        {
                            FranchiseId = new Guid("1c816099-c827-4a28-ab64-f152dc44ba13"),
                            Description = "Trilogy based on the books written by J.R.R. Tolkien.",
                            Name = "The Lord of the Rings"
                        },
                        new
                        {
                            FranchiseId = new Guid("a6c6bad1-aaee-4c5c-9f40-1b45e2c3fe3c"),
                            Description = "Series based on the books written by J.K. Rowling.",
                            Name = "Harry Potter"
                        });
                });

            modelBuilder.Entity("MovieInfoAPI.Models.Domain.Movie", b =>
                {
                    b.Property<Guid>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("FranchiseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Trailer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MovieId");

                    b.HasIndex("FranchiseId");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("CharacterMovie", b =>
                {
                    b.HasOne("MovieInfoAPI.Models.Domain.Character", null)
                        .WithMany()
                        .HasForeignKey("CharactersCharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieInfoAPI.Models.Domain.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieInfoAPI.Models.Domain.Movie", b =>
                {
                    b.HasOne("MovieInfoAPI.Models.Domain.Franchise", "Franchise")
                        .WithMany("Movies")
                        .HasForeignKey("FranchiseId");

                    b.Navigation("Franchise");
                });

            modelBuilder.Entity("MovieInfoAPI.Models.Domain.Franchise", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
