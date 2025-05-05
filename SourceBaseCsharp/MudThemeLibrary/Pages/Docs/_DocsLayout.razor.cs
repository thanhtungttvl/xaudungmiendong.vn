using Microsoft.AspNetCore.Components;

namespace MudThemeLibrary.Pages.Docs
{
    public partial class _DocsLayout
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
