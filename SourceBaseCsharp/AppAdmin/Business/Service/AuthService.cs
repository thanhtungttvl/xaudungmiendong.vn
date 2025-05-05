using AppAdmin.Business.Providers;
using AppAdmin.Business.Service.Base;
using AppShare.Models.Auth;
using AppShare.Models.User;
using Microsoft.AspNetCore.Components.Authorization;
using MudThemeLibrary.Handlers;

namespace AppAdmin.Business.Service
{
    public class AuthService : BaseService
    {
        private readonly SecureLocalStorageHandler _secureLocalStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, SecureLocalStorageHandler secureLocalStorage, AuthenticationStateProvider authenticationStateProvider) : base(httpClientFactory, configuration)
        {
            _secureLocalStorage = secureLocalStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> LoginAsync(AuthLoginRequestModel model)
        {
            return await HttpClientBase().PostAsync($"api/auth/login", RequestContent(model));
        }

        public async Task<HttpResponseMessage> RegisterAsync(AuthRegisterRequestModel model)
        {
            return await HttpClientBase().PostAsync($"api/auth/register", RequestContent(model));
        }

        public async Task<HttpResponseMessage> RecoverPasswordAsync(string email)
        {
            return await HttpClientBase().GetAsync($"api/auth/recover-password/{email}");
        }

        public async Task<HttpResponseMessage> ResetPasswordAsync(AuthResetPasswordRequestModel model)
        {
            return await HttpClientBase().PostAsync($"api/auth/reset-password", RequestContent(model));
        }

        /// <summary>
        /// Lấy thông tin token login từ [user-login] ở LocalStorage
        /// Nếu token tồn tại thì gọi lên server check login
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetInfoAsync()
        {
            // Lấy token info của player
            var local = (LocalAuthenticationProvider)_authenticationStateProvider;
            var token = await local.GetToken();

            // Nếu token không có thì player chưa đăng nhập hoặc chưa tạo thông tin người chơi là khách
            if (string.IsNullOrEmpty(token) is true)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }

            // Nếu có token thì tiến hành login bằng token đó
            return await HttpClientBase().GetAsync($"api/auth/login/token");
        }

        /// <summary>
        /// Lưu token login vào [user-login] ở LocalStorage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateInfoAsync(UserModel model)
        {
            await _secureLocalStorage.SetItemAsync("user-login", model);
        }
    }
}
