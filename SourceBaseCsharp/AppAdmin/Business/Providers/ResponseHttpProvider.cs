using CurrieTechnologies.Razor.SweetAlert2;
using AppShare.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;
using System.Net.Http.Json;

namespace AppAdmin.Business.Providers
{
    public interface IResponseHttpProvider
    {
        Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response);
    }

    public class ResponseHttpProvider : IResponseHttpProvider
    {
        private readonly ISnackbar _snackbar;
        private readonly SweetAlertService _swal;
        private readonly NavigationManager _navigation;

        public ResponseHttpProvider(ISnackbar snackbar, SweetAlertService swal, NavigationManager navigation)
        {
            _snackbar = snackbar;
            _swal = swal;
            _navigation = navigation;
        }

        public async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch (Exception ex)
                {
                    await _swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "Oops...",
                        Html = $"<p>Lỗi hệ thống, vui lòng liện hệ quản trị viên để biết thêm. Xin cám ơn.</p><p><b>{ex.Message}</b><p>",
                        Icon = SweetAlertIcon.Error,
                        AllowOutsideClick = false,
                        Footer = @"<a target='_blank' href='https://www.facebook.com/thanhtungttvl'>Liên hệ với quản trị viên</a>"
                    });

                    return default;
                }
            }

            switch (response.StatusCode)
            {
                #region BadRequest, NotFound
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.NotFound:
                    var error = await response.Content.ReadFromJsonAsync<ErrorModel>();
                    if (error is not null)
                    {
                        _snackbar.Add(new MarkupString(error.Title), Severity.Error);
                    }
                    break;
                #endregion

                #region Unauthorized
                case HttpStatusCode.Unauthorized:
                    var result = await _swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "Oops...",
                        Html = "Bạn chưa đăng nhập, hoặc phiên đăng nhập của bạn đã hết hạn, bạn cần đăng nhập lại để tiếp tục.",
                        Icon = SweetAlertIcon.Error,
                        AllowOutsideClick = false,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Vâng, đến trang đăng nhập!",
                        CancelButtonText = "Không"
                    });

                    if (!string.IsNullOrEmpty(result.Value))
                    {
                        var returnUrl = _navigation.ToBaseRelativePath(_navigation.Uri);
                        _navigation.NavigateTo($"/logout?returnUrl={Uri.EscapeDataString(returnUrl)}");
                    }
                    break;
                #endregion

                #region Default
                default:
                    await _swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "Oops...",
                        Html = "Lỗi hệ thống, vui lòng liện hệ quản trị viên để biết thêm. Xin cám ơn.",
                        Icon = SweetAlertIcon.Error,
                        AllowOutsideClick = false,
                        Footer = @"<a target='_blank' href='https://www.facebook.com/thanhtungttvl'>Liên hệ với quản trị viên</a>"
                    });
                    break;
                    #endregion
            }

            return default;
        }
    }
}
