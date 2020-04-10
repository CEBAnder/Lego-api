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
    }
}
