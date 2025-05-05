using AppAdmin.StateContainer;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AppAdmin.Layout
{
    public partial class MainLayout : IAsyncDisposable
    {
        [Inject] private ThemeStateContainer _themeStateContainer { get; set; } = default!;

        private MudTheme _myCustomTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Background = "rgba(0, 0, 0, 0.05)",
                Primary = "#003a88",
                AppbarBackground = "#003a88",
            }
        };

        protected override async Task OnInitializedAsync()
        {
            _themeStateContainer.OnStateChange += StateHasChanged;

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _themeStateContainer.IsDarkMode = await SecureLocalStorage.GetItemAsync<bool>("dark-mode");
            }
        }

        public ValueTask DisposeAsync()
        {
            _themeStateContainer.OnStateChange -= StateHasChanged;
            return new ValueTask();
        }
    }
}
