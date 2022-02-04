using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MovieInfoAPI.Models.Domain;

namespace MovieInfoAPI.Model
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Franchise> Franchises { get; set; }

        public MovieDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        public MovieDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Franchise lotr = new Franchise()
            {
                FranchiseId = 1,
                Name = "The Lord of the Rings",
                Description = "Trilogy based on the books written by J.R.R. Tolkien."
            };
            Franchise hp = new Franchise
            {
                FranchiseId = 2,
                Name = "Harry Potter",
                Description = "Series based on the books written by J.K. Rowling."
            };

            modelBuilder.Entity<Franchise>().HasData(lotr);
            modelBuilder.Entity<Franchise>().HasData(hp);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data source=ND-5CG9030MCG\\SQLEXPRESS; Initial Catalog=MovieInfoAPIDB; Integrated Security=True;");
        }
    }
}
