using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AppAdmin
{
    public partial class Program
    {
        private static async Task AddConfiguration(WebAssemblyHostBuilder builder)
        {
            // Tải tệp cấu hình dựa trên môi trường
            using var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

            // Kiểm tra môi trường hiện tại
            var environment = builder.HostEnvironment.Environment;

            string configFile = environment == "Development" ? "appsettings.Development.json" : "appsettings.json";

            using var response = await httpClient.GetAsync(configFile);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to load {configFile}: {response.StatusCode}");
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)  // Thêm file JSON vào cấu hình
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}
