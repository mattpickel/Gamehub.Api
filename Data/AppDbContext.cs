using Microsoft.EntityFrameworkCore;
using Gamehub.Api.Models;

namespace Gamehub.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "SampleUser",
                FirstName = "Sample",
                LastName = "User",
                TotalGamesPlayed = 10,
                TotalGamesWon = 5,
                LongestStreak = 3,
                CurrentStreak = 2
            });
        }
    }
}
