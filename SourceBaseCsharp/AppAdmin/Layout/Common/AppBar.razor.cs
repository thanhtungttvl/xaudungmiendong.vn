using AppAdmin.StateContainer;
using Microsoft.AspNetCore.Components;

namespace AppAdmin.Layout.Common
{
    public partial class AppBar : IAsyncDisposable
    {
        [Inject] ThemeStateContainer _themeStateContainer { get; set; } = default!;
        [Inject] UserStateContainer _userStateContainer { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            _themeStateContainer.OnStateChange += StateHasChanged;
            _userStateContainer.OnStateChange += StateHasChanged;

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _userStateContainer.GetInfo();
            }
        }

        private void DrawerToggle()
        {
            _themeStateContainer.DrawerOpenAppBar = !_themeStateContainer.DrawerOpenAppBar;
        }

        public ValueTask DisposeAsync()
        {
            _themeStateContainer.OnStateChange -= StateHasChanged;
            _userStateContainer.OnStateChange -= StateHasChanged;
            return new ValueTask();
        }
    }
}
