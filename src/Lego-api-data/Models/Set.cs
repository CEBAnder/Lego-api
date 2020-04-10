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
    }
}
