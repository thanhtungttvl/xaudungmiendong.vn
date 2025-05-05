using Microsoft.AspNetCore.Components;

namespace MudThemeLibrary.Theme
{
    public partial class CardArticleTheme
    {
        [Parameter]
        public RenderFragment? CardContent { get; set; }

        [Parameter]
        public string? Title { get; set; } = "This is some title";

        [Parameter]
        public string Link { get; set; } = "#";

        [Parameter]
        public string Banner { get; set; } = "_content/MudThemeLibrary/images/default-img.svg";

        [Parameter]
        public RenderFragment? CardFooter { get; set; }

        [Parameter]
        public string AuthorAvatar { get; set; } = "_content/MudThemeLibrary/images/default-img-square.svg";

        [Parameter]
        public string AuthorName { get; set; } = "Michelle Doe";
        [Parameter]
        public string AuthorDate { get; set; } = "18 Oct 2023";
        [Parameter]
        public string AuthorLink { get; set; } = "#";
    }
}
