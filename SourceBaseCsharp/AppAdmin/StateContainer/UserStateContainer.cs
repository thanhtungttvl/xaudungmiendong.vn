using AppAdmin.Business.Providers;
using AppAdmin.Business.Service;
using CurrieTechnologies.Razor.SweetAlert2;
using AppShare.Models.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;

namespace AppAdmin.StateContainer
{
    public class UserStateContainer
    {
        private readonly IResponseHttpProvider _responseHttp;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISnackbar _snackbar;
        private readonly SweetAlertService _swal;
        private readonly NavigationManager _navigation;
        private readonly UserService _userService;

        public UserModel User { get; set; } = new();
        public bool IsLoggedIn { get; set; } = false;

        public event Action? OnStateChange;
        public void NotifyStateChanged() => OnStateChange?.Invoke();

        public UserStateContainer(IResponseHttpProvider responseHttp, AuthenticationStateProvider authStateProvider,
            ISnackbar snackbar, SweetAlertService swal, NavigationManager navigation, UserService userService)
        {
            _responseHttp = responseHttp;
            _authStateProvider = authStateProvider;
            _snackbar = snackbar;
            _swal = swal;
            _navigation = navigation;
            _userService = userService;
        }

        public async Task GetInfo()
        {
            // Lấy thông tin player
            var response = await _userService.GetInfoAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //check response từ api trả về
                var user = await _responseHttp.ReadFromJsonAsync<UserModel>(response);
                if (user is not null)
                {
                    if (user.Status == UserEnum.StatusOption.Locked)
                    {
                        var result = await _swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "Oops...",
                            Html = "Tài khoản của bạn đã bị khóa, liên hệ quản trị viên để biết thêm.",
                            Icon = SweetAlertIcon.Error,
                            AllowOutsideClick = false,
                            Footer = @"<a target='_blank' href='https://www.facebook.com/thanhtungttvl'>Liên hệ với quản trị viên</a>"
                        });

                        if (string.IsNullOrEmpty(result.Value) is false)
                        {
                            var returnUrl = _navigation.ToBaseRelativePath(_navigation.Uri);
                            _navigation.NavigateTo($"/logout");
                        }
                    }
                    else
                    {
                        await UpdateUser(user);
                    }
                }
            }
            else
            {
                var authStateProvider = (LocalAuthenticationProvider)_authStateProvider;
                await authStateProvider.NotifyUserLogout();
            }
        }

        public void SetInfo(UserModel user)
        {
            User = user;
            NotifyStateChanged();
        }

        public async Task UpdateUser(UserModel user)
        {
            User = user;

            var userClaims = (await _authStateProvider.GetAuthenticationStateAsync()).User;
            if (userClaims.Identity is not null && userClaims.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(user.Email) is true)
                {
                    IsLoggedIn = false;
                }
                else
                {
                    IsLoggedIn = true;
                }
            }

            NotifyStateChanged();
        }
    }
}
