using AppAdmin.Business.Service;
using AppAdmin.StateContainer;
using AppShare.Models.Auth;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudThemeLibrary.Helpers;

namespace AppAdmin.Pages.Auth
{
    public partial class RegisterPage : IAsyncDisposable
    {
        private bool _processing = false;
        private AuthRegisterRequestModel _userRegister { get; set; } = new();

        [Inject] private UserService _userService { get; set; } = default!;
        [Inject] private UserStateContainer _userStateContainer { get; set; } = default!;
        [Inject] private ThemeStateContainer _themeStateContainer { get; set; } = default!;

        private async Task OnValidSubmit()
        {
            _processing = true;
            _themeStateContainer.IsLoading = true;

            var response = await _userService.RegisterAsync(_userRegister);
            var result = await ResponseHttp.ReadFromJsonAsync<Guid?>(response);
            if (result is not null)
            {
                Snackbar.Add(new MarkupString("Đăng ký tài khoản thành công.<br/>Chào mừng bạn đến với thế giới trò chơi của <b>SGVN<b/>."), Severity.Success);

                Navigation.NavigateTo("/login");
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
