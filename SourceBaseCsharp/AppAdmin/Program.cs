using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text;

namespace AppAdmin
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // kích hoạt hỗ trợ cho các mã hóa bổ sung, bao gồm cả mã hóa Cyrillic
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            await AddConfiguration(builder);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAuthorizationCore();

            Api(builder);
            AddStateContainer(builder);
            AddService(builder);

            await builder.Build().RunAsync();
        }
    }
}
