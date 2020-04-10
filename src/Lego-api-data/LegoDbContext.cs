using Lego_api_data.Helpers;
using Lego_api_data.Models;
using Microsoft.EntityFrameworkCore;

namespace Lego_api_data
{
    public class LegoDbContext : DbContext
    {
        public LegoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Theme> Themes { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ConfigureSets()
                .ConfigureThemes();
        }
    }
}
