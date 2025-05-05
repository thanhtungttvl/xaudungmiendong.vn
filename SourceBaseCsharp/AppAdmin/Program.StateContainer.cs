using AppAdmin.StateContainer;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AppAdmin
{
    public partial class Program
    {
        private static void AddStateContainer(WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddScoped<HubMesssgeStateContainer>()
                .AddScoped<ThemeStateContainer>()
                .AddScoped<UserStateContainer>();
        }
    }
}
