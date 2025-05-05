using AppAdmin.Business.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace AppAdmin.Business.Handlers
{
    public class RequestHttpHandler : DelegatingHandler
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public RequestHttpHandler(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = request.RequestUri!.AbsolutePath.ToLower();
            if (url.Contains("auth/login") is true && url.Contains("auth/login/token") is false)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var local = (LocalAuthenticationProvider)_authenticationStateProvider;
            var token = await local.GetToken();

            if (string.IsNullOrEmpty(token) is false)
            {
                // request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
