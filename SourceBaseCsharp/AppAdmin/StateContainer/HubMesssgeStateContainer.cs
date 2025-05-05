using AppAdmin.Business.Providers;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace AppAdmin.StateContainer
{
    public class HubMesssgeStateContainer
    {
        private readonly SweetAlertService _swal;
        private readonly NavigationManager _navigation;
        private readonly IResponseHttpProvider _responseHttp;
        private readonly UserStateContainer _userState;
        private readonly AuthenticationStateProvider _authStateProvider;

        public HubMesssgeStateContainer(SweetAlertService swal, NavigationManager navigation, IResponseHttpProvider responseHttp,
            UserStateContainer userState, AuthenticationStateProvider authStateProvider)
        {
            _swal = swal;
            _navigation = navigation;
            _responseHttp = responseHttp;
            _userState = userState;
            _authStateProvider = authStateProvider;
        }

        /// <summary>
        /// ConnectSuccess
        /// </summary>
        /// <param name="hub"></param>
        /// <returns></returns>
        public async Task ConnectSuccess(HubConnection hub)
        {
            await _swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Success,
                Title = "ConnectSuccess",
                Html = $"<p>ConnectionId: <b>{hub.ConnectionId}</b><p>"
            });
        }

        /// <summary>
        /// Bắt đầu kết nối lên Hub
        /// </summary>
        /// <param name="hub"></param>
        /// <returns></returns>
        public async Task StartConnect(HubConnection hub)
        {
            try
            {
                await hub.StartAsync();
            }
            catch (Exception ex)
            {
                await _swal.FireAsync(new SweetAlertOptions
                {
                    Icon = SweetAlertIcon.Question,
                    Title = "Oops...",
                    Html = $"<p>Lỗi hệ thống, vui lòng liện hệ quản trị viên để biết thêm. Xin cám ơn.</p><p><b>{ex.Message}</b><p>",
                    ShowCancelButton = false,
                    AllowOutsideClick = false,
                    Footer = @"<a target=""_blank"" href=""https://www.facebook.com/thanhtungttvl"">Liên hệ với quản trị viên</a>"
                });
            }
        }
    }
}
