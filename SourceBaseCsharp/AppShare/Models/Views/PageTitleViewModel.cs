namespace AppShare.Models.Views
{
    public class PageTitleViewModel
    {
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Url của background
        /// </summary>
        public string Background { get; set; } = string.Empty;
        public Dictionary<string, string> Breadcrumbs { get; set; } = null!;
    }
}
