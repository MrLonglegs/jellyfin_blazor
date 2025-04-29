using System.Globalization;
using Microsoft.JSInterop;

namespace jellyfin_blazor.Services
{
    public interface ILocalStorage
    {
        Task SetItemAsync(string key, string value);
        Task<string?> GetItemAsync(string key);
        Task RemoveItemAsync(string key);
    }

    public class LocalStorageProvider : ILocalStorage
    {
        private readonly IJSRuntime _jsRuntime;
        public LocalStorageProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        public async Task<string?> GetItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var result = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            return result ?? String.Empty;
        }
        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
