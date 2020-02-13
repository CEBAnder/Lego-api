using Microsoft.EntityFrameworkCore;

namespace Lego_api_data.Models
{
    public class Set
    {
        public int SetId { get; set; }
        public string SetNumber { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int NumberOfParts { get; set; }
        public int ThemeId { get; set; }

        public Theme SetTheme { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var setBuilder = modelBuilder.Entity<Set>();

            setBuilder
                .Property(x => x.SetId)
                .HasColumnName("set_id");

            setBuilder
                .Property(x => x.SetNumber)
                .HasColumnName("set_num");

            setBuilder
                .Property(x => x.Name)
                .HasColumnName("name");

            setBuilder
                .Property(x => x.Year)
                .HasColumnName("year");

            setBuilder
                .Property(x => x.NumberOfParts)
                .HasColumnName("num_parts");

            setBuilder
                .HasOne(x => x.SetTheme)
                .WithMany(x => x.Sets)
                .HasForeignKey(x => x.ThemeId);
        }
    }
}
