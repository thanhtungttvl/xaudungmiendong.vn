using Microsoft.JSInterop;

namespace MudThemeLibrary.Handlers
{
    public class CryptoInteropHandler : IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private Lazy<Task<IJSObjectReference>> _module;

        public CryptoInteropHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _module = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/MudThemeLibrary/js/crypto-interop.js").AsTask());
        }

        // Encrypt string
        public async Task<string> EncryptAsync(string plainText, string key)
        {
            var module = await _module.Value; // Lấy module đã được khởi tạo hoặc khởi tạo lần đầu
            return await module.InvokeAsync<string>("CryptoInterop.encrypt", plainText, key);
        }

        // Decrypt string
        public async Task<string> DecryptAsync(string cipherText, string key)
        {
            var module = await _module.Value; // Lấy module đã được khởi tạo hoặc khởi tạo lần đầu
            return await module.InvokeAsync<string>("CryptoInterop.decrypt", cipherText, key);
        }

        // Dispose the JavaScript module
        public async ValueTask DisposeAsync()
        {
            if (_module.IsValueCreated)
            {
                var module = await _module.Value;
                await module.DisposeAsync();
            }
        }
    }
}
