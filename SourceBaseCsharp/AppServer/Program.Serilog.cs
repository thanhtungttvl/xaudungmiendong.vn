using Serilog.Events;
using Serilog;

namespace AppServer
{
    public partial class Program
    {
        private static void AddSerilog(WebApplicationBuilder builder)
        {
            var appServer = builder.Configuration["MySerilog:FileName:AppServer"]!;
            var appAdmin = builder.Configuration["MySerilog:FileName:AppAdmin"]!;

            var now = DateTime.Now;
            var date = $"{now:ddMMyyyy}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()

                .WriteTo.File(
                    path: $"logs/log_{date}.txt",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    shared: true) // Ghi all log

                .WriteTo.Logger(orderLog => orderLog
                    .WriteTo.File(path: $"logs/{appServer}_{date}.txt",
                                  restrictedToMinimumLevel: LogEventLevel.Information,
                                  outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                  shared: true)
                    .Filter.ByIncludingOnly(logEvent => logEvent.Properties.ContainsKey("Server"))) // Chỉ ghi log liên quan đến Server

                .WriteTo.Logger(orderLog => orderLog
                    .WriteTo.File(path: $"logs/{appAdmin}_{date}.txt",
                                  restrictedToMinimumLevel: LogEventLevel.Information,
                                  outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                  shared: true)
                    .Filter.ByIncludingOnly(logEvent => logEvent.Properties.ContainsKey("Client"))) // Chỉ ghi log liên quan đến Client

                .CreateLogger();
        }

        private static void UseSerilog(WebApplication app)
        {
            // Xử lý khi ứng dụng dừng
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(() =>
            {
                Log.ForContext("Server", true).Information("Application is shutting down...");
                Log.CloseAndFlush();
            });
        }
    }
}
