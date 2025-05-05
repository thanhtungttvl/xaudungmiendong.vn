using AppAdmin.Business.Providers;
using AppAdmin.Business.Service;
using AppAdmin.StateContainer;
using AppShare.Models.Auth;
using AppShare.Models.User;
using Microsoft.AspNetCore.Components;
using MudThemeLibrary.Helpers;

namespace AppAdmin.Pages.Auth
{
    public partial class LoginPage : IAsyncDisposable
    {
        private bool _processing = false;
        private AuthLoginRequestModel _userLogin { get; set; } = new();

        [Inject] private UserService _userService { get; set; } = default!;
        [Inject] private UserStateContainer _userStateContainer { get; set; } = default!;
        [Inject] private ThemeStateContainer _themeStateContainer { get; set; } = default!;

        private async Task OnValidSubmit()
        {
            _processing = true;
            _themeStateContainer.IsLoading = true;

            var response = await _userService.LoginAsync(_userLogin);
            var result = await ResponseHttp.ReadFromJsonAsync<UserModel>(response);
            if (result is not null)
            {
                var authStateProvider = (LocalAuthenticationProvider)AuthStateProvider;
                await authStateProvider.NotifyAuthenticationState(result);

                await _userStateContainer.UpdateUser(result);

                Navigation.NavigateTo($"/{Navigation.QueryStringUrl("returnUrl")}");
            }

            _themeStateContainer.IsLoading = false;
            _processing = false;
        }

        protected override async Task OnInitializedAsync()
        {
            await _userStateContainer.GetInfo();
            if (_userStateContainer.IsLoggedIn)
            {
                Navigation.NavigateTo($"/{Navigation.QueryStringUrl("returnUrl")}");
                return;
            }

            _userStateContainer.OnStateChange += StateHasChanged;
            _themeStateContainer.OnStateChange += StateHasChanged;

            await base.OnInitializedAsync();
        }

        public ValueTask DisposeAsync()
        {
            _userStateContainer.OnStateChange -= StateHasChanged;
            _themeStateContainer.OnStateChange -= StateHasChanged;
            return new ValueTask();
        }
    }
}
