using Blazored.LocalStorage;
using System.Text.Json;

namespace MudThemeLibrary.Handlers
{
    public class SecureLocalStorageHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly CryptoInteropHandler _cryptoInterop;
        private readonly string _encryptionKey;

        public SecureLocalStorageHandler(ILocalStorageService localStorage, CryptoInteropHandler cryptoInterop, string encryptionKey)
        {
            _localStorage = localStorage;
            _cryptoInterop = cryptoInterop;
            _encryptionKey = encryptionKey;
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            string? jsonData;

            if (value is string str)
            {
                // Nếu value là chuỗi, gán trực tiếp
                jsonData = str;
            }
            else
            {
                // Serialize object to JSON
                jsonData = JsonSerializer.Serialize(value);
            }

            // Encrypt data
            var encryptedData = await _cryptoInterop.EncryptAsync(jsonData, _encryptionKey);

            // Save encrypted data to LocalStorage
            await _localStorage.SetItemAsync(key, encryptedData);
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            // Get encrypted data from LocalStorage
            var encryptedData = await _localStorage.GetItemAsync<string>(key);

            if (string.IsNullOrEmpty(encryptedData))
            {
                return default;
            }

            // Decrypt data
            var jsonData = await _cryptoInterop.DecryptAsync(encryptedData, _encryptionKey);

            // Deserialize JSON to object
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _localStorage.RemoveItemAsync(key);
        }
    }
}
