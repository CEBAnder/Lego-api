using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lego_api_data.Models
{
    public class Theme
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public Theme ParentTheme { get; set; }
        public List<Theme> Themes { get; set; }
        public List<Set> Sets { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var themeBuilder = modelBuilder.Entity<Theme>();

            themeBuilder
                .Property(x => x.ThemeId)
                .HasColumnName("theme_id");

            themeBuilder
                .Property(x => x.Name)
                .HasColumnName("name");

            themeBuilder
                .Property(x => x.ParentId)
                .HasColumnName("parent_id");

            themeBuilder
                .HasOne(x => x.ParentTheme)
                .WithMany(x => x.Themes)
                .HasForeignKey(x => x.ParentId)
                .HasConstraintName("FK_parent_id_theme_id");

            themeBuilder
                .HasMany(x => x.Sets)
                .WithOne(x => x.SetTheme)
                .HasForeignKey(x => x.ThemeId)
                .HasConstraintName("FK__Sets__theme_id__38996AB5");

            themeBuilder
                .Ignore(x => x.ParentTheme);

            themeBuilder
                .Ignore(x => x.Sets);

            themeBuilder
                .Ignore(x => x.Themes);
        }
    }
}
