using AppServer.Hubs;

namespace AppServer
{
    public partial class Program
    {
        private static void AddHub(WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR(config => config.EnableDetailedErrors = true);
        }

        private static void UseHub(WebApplication app)
        {
            app.MapHub<UserHub>("/hub/user");
        }
    }
}
