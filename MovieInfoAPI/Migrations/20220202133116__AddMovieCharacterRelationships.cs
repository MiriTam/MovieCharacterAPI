using Microsoft.EntityFrameworkCore.Migrations;
using MovieInfoAPI.Model;
using MovieInfoAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MovieInfoAPI.Migrations
{
    public partial class _AddMovieCharacterRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (MovieDbContext context = new MovieDbContext())
            {
                // Lord of the Rings movies
                Movie lotr1 = context.Movies.Include(m => m.Characters).FirstOrDefault(m => m.Title == "The Fellowship of the Ring");
                Movie lotr2 = context.Movies.Include(m => m.Characters).FirstOrDefault(m => m.Title == "The Two Towers");

                // Harry Potter movies
                Movie hp1 = context.Movies.Include(m => m.Characters).FirstOrDefault(m => m.Title == "Harry Potter and the Philosopher's Stone");
                Movie hp4 = context.Movies.Include(m => m.Characters).FirstOrDefault(m => m.Title == "Harry Potter and the Goblet of Fire");
                Movie hp7 = context.Movies.Include(m => m.Characters).FirstOrDefault(m => m.Title == "Harry Potter and the Deathly Hallows");

                // Lord of the Rings characters
                Character sam = context.Characters.FirstOrDefault(c => c.Name == "Samwise Gamgee");
                Character galadriel = context.Characters.FirstOrDefault(c => c.Name == "Galadriel");
                Character gandalf = context.Characters.FirstOrDefault(c => c.Name == "Gandalf");

                // Harry Potter characters
                Character hermione = context.Characters.FirstOrDefault(c => c.Name == "Hermione Granger");
                Character hedwig = context.Characters.FirstOrDefault(c => c.Name == "Hedwig");
                Character dobby = context.Characters.FirstOrDefault(c => c.Name == "Dobby");

                // Add Lord of the Rings characters
                lotr1.Characters.Add(sam);
                lotr1.Characters.Add(galadriel);
                lotr1.Characters.Add(gandalf);
                lotr2.Characters.Add(sam);
                lotr2.Characters.Add(gandalf);

                // Add Harry Potter characters
                hp1.Characters.Add(hermione);
                hp1.Characters.Add(hedwig);
                hp4.Characters.Add(hermione);
                hp4.Characters.Add(hedwig);
                hp4.Characters.Add(dobby);
                hp7.Characters.Add(hermione);
                hp7.Characters.Add(hedwig);
                hp7.Characters.Add(dobby);

                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
