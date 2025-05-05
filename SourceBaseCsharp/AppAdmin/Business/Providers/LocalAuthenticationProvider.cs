using AppShare.Core;
using AppShare.Models.User;
using Microsoft.AspNetCore.Components.Authorization;
using MudThemeLibrary.Handlers;
using System.Security.Claims;

namespace AppAdmin.Business.Providers
{
    public class LocalAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly SecureLocalStorageHandler _secureLocalStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public LocalAuthenticationProvider(SecureLocalStorageHandler secureLocalStorage)
        {
            _secureLocalStorage = secureLocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userLogin = await _secureLocalStorage.GetItemAsync<UserModel>("user-login");
                if (userLogin == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claims = new List<Claim>
                {
                    new Claim("Id", userLogin.Id.ToString()),
                    new Claim("Name", userLogin.Name),
                    new Claim(ClaimTypes.Role, userLogin.Role.GetDescription()),
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task NotifyAuthenticationState(UserModel? userLogin)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userLogin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", userLogin.Id.ToString()),
                    new Claim("Name", userLogin.Name),
                    new Claim(ClaimTypes.Role, userLogin.Role.GetDescription()),
                };

                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));

                userLogin.ExpiryTimeStamp = DateTime.Now.AddSeconds(userLogin.ExpiresIn);
                await _secureLocalStorage.SetItemAsync("user-login", userLogin);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _secureLocalStorage.RemoveItemAsync("user-login");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetToken()
        {
            var result = string.Empty;
            try
            {
                var userLogin = await _secureLocalStorage.GetItemAsync<UserModel>("user-login");
                if (userLogin != null && DateTime.Now < userLogin.ExpiryTimeStamp)
                {
                    result = userLogin.Token;
                }
            }
            catch { }

            return result;
        }

        public async Task NotifyUserLogout()
        {
            await _secureLocalStorage.RemoveItemAsync("user-login");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
