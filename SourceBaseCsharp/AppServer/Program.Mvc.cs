using System.Text.Json.Serialization;

namespace AppServer
{
    public partial class Program
    {
        private static void AddMvc(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });
        }

        private static void UseMvc(WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
