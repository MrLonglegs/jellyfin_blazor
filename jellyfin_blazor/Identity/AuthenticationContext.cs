using jellyfin_blazor.Services;
using Newtonsoft.Json;

namespace jellyfin_blazor.Identity
{
    public class AuthenticationContext
    {
        private dynamic _authPayload;
        private readonly ILocalStorage _localStorage;
        private readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://jellyfin.brueffer24.de/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public AuthenticationContext(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/Users/AuthenticateByName");
            request.Headers.Add("Authorization", $"MediaBrowser Client=\"jellyfin_blazor\", Device=\"Jellyfin Blazor WebClient\", DeviceId=\"{await GetDeviceID()}\", Version=\"1.0.0\"");
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                Username = username,
                Pw = password,
            }), System.Text.Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(request, CancellationToken.None);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                _authPayload = JsonConvert.DeserializeObject<dynamic>(content);
                _localStorage.SetItemAsync("AuthPayload", content);
                return true;
            }
            else
            {
                throw new Exception("Authentication failed");
            }
        }
        public async Task<string> GetAccessToken()
        {
            if (_authPayload == null)
            {
                var content = await _localStorage.GetItemAsync("AuthPayload");
                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("No authentication payload found");
                    return String.Empty;
                }
                _authPayload = JsonConvert.DeserializeObject<dynamic>(content);
            }
            return _authPayload.AccessToken ?? String.Empty;
        }

        public async Task<string> GetDeviceID()
        {
            var deviceID = await _localStorage.GetItemAsync("DeviceID");
            if (String.IsNullOrEmpty(deviceID)) {
                deviceID = Guid.NewGuid().ToString();
                await _localStorage.SetItemAsync("DeviceID", deviceID);
            }
            return deviceID;
        }
    }
}
