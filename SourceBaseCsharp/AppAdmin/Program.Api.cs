using AppAdmin.Business.Handlers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AppAdmin
{
    public partial class Program
    {
        private static void Api(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient(builder.Configuration["Servers:Default:Name"]!, options =>
            {
                var api = builder.Configuration["Servers:Default:Url"];
                if (string.IsNullOrEmpty(api) is false)
                {
                    options.BaseAddress = new Uri(api);
                }
            }).AddHttpMessageHandler<RequestHttpHandler>();
            builder.Services.AddScoped<RequestHttpHandler>();
        }
    }
}
