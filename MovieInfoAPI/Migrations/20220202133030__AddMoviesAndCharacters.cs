using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MovieInfoAPI.Model;
using MovieInfoAPI.Models.Domain;
using System;
using System.Linq;

namespace MovieInfoAPI.Migrations
{
    public partial class _AddMoviesAndCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (MovieDbContext context = new MovieDbContext())
            {
                Franchise lotr = context.Franchises.FirstOrDefault(f => f.Name == "The Lord of the Rings");
                Franchise hp = context.Franchises.FirstOrDefault(f => f.Name == "Harry Potter");

                migrationBuilder.InsertData(
                    table: "Movie",
                    columns: new[] { "MovieId", "Title", "Genre", "ReleaseYear", "Director", "FranchiseId" },
                    values: new object[,]
                    {
                        { Guid.NewGuid(), "Harry Potter and the Philosopher's Stone", "Fantasy", 2001, "Chris Columbus", hp.FranchiseId },
                        { Guid.NewGuid(), "Harry Potter and the Goblet of Fire", "Fantasy", 2005, "Mike Newell", hp.FranchiseId },
                        { Guid.NewGuid(), "Harry Potter and the Deathly Hallows", "Fantasy", 2011, "David Yates", hp.FranchiseId },
                        { Guid.NewGuid(), "The Fellowship of the Ring", "Fantasy", 2001, "Peter Jackson", lotr.FranchiseId },
                        { Guid.NewGuid(), "The Two Towers", "Fantasy", 2002, "Peter Jackson", lotr.FranchiseId }
                    });

                migrationBuilder.InsertData(
                    table: "Character",
                    columns: new[] { "CharacterId", "Name", "Alias", "Gender" },
                    values: new object[,]
                    {
                        { Guid.NewGuid(), "Samwise Gamgee", "Sam", "Male" },
                        { Guid.NewGuid(), "Galadriel", "Lady of Ligth", "Female" },
                        { Guid.NewGuid(), "Gandalf", "The Gray", "Male" },
                        { Guid.NewGuid(), "Hermione Granger", "The smartest witch her age", "Female" },
                        { Guid.NewGuid(), "Hedwig", "Snow goddess of the skies", "Female" },
                        { Guid.NewGuid(), "Dobby", "A free elf", "Male" }
                    });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
