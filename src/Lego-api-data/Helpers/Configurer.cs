using Lego_api_data.Models;
using Microsoft.EntityFrameworkCore;

namespace Lego_api_data.Helpers
{
    public static class Configurer
    {
        public static ModelBuilder ConfigureSets(this ModelBuilder modelBuilder)
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

            return modelBuilder;
        }

        public static ModelBuilder ConfigureThemes(this ModelBuilder modelBuilder)
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
                .HasConstraintName("FK_sets_themes_theme_id");

            themeBuilder
                .Ignore(x => x.ParentTheme);

            themeBuilder
                .Ignore(x => x.Sets);

            themeBuilder
                .Ignore(x => x.Themes);

            return modelBuilder;
        }
    }
}
