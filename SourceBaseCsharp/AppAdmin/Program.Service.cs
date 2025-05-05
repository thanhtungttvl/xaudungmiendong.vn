using AppAdmin.Business.Providers;
using AppAdmin.Business.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudThemeLibrary.Core;

namespace AppAdmin
{
    public partial class Program
    {
        private static void AddService(WebAssemblyHostBuilder builder)
        {
            // Khai báo service MudThemeLibrary
            builder.Services.AddMudThemeLibraryServices();

            builder.Services
                .AddScoped<AuthenticationStateProvider, LocalAuthenticationProvider>()
                .AddScoped<IResponseHttpProvider, ResponseHttpProvider>();

            builder.Services
                .AddScoped<UserService>();
        }
    }
}
