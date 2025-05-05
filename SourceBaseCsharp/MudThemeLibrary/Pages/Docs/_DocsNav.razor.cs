using MudBlazor;

namespace MudThemeLibrary.Pages.Docs
{
    public partial class _DocsNav : IAsyncDisposable
    {
        private bool _open = true;
        private DrawerVariant _variant = DrawerVariant.Mini;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void HandleOpenNav()
        {
            _open = !_open;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
