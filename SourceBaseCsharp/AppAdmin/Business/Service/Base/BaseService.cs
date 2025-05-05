using System.Text.Json;
using System.Text;

namespace AppAdmin.Business.Service.Base
{
    public class BaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BaseService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public HttpClient HttpClientBase()
        {
            return _httpClientFactory.CreateClient(_configuration["Servers:Default:Name"]!);
        }

        public StringContent RequestContent<T>(T value)
        {
            var jsonPayload = JsonSerializer.Serialize(value);

            return new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        }
    }
}
