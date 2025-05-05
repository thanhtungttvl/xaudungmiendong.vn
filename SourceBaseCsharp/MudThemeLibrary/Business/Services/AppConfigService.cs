using Microsoft.Extensions.Configuration;

namespace MudThemeLibrary.Business.Services
{
    public interface IAppConfigService
    {
        public string GetValue(string key);
    }

    public class AppConfigService : IAppConfigService
    {
        private readonly IConfiguration _configuration;

        public AppConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key) => _configuration[key] ?? string.Empty;
    }
}
