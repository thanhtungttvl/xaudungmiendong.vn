using AppServer.Business.Service;
using AppServer.Business.Service.Background;

namespace AppServer
{
    public partial class Program
    {
        private static void AddService(WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<TestBackgroundService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
        }
    }
}
