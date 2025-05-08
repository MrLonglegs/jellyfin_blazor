using System.Net.Http.Json;
using jellyfin_blazor.Identity;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace jellyfin_blazor.Services
{
    public class HttpService : HttpClient
    {
        private readonly IdentityContext _authenticationContext;
        private readonly NavigationManager _navigationManager;
        public HttpService(IdentityContext authenticationContext, NavigationManager navigationManager)
        {
            _authenticationContext = authenticationContext;
            _navigationManager = navigationManager;
            BaseAddress = new Uri("https://jellyfin.brueffer24.de");
        }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var deviceID = await _authenticationContext.GetDeviceID();
            var accessToken = await _authenticationContext.GetAccessToken();

            if (String.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("No access token found. Redirecting to login");
                _navigationManager.NavigateTo("/login");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            request.Headers.Add("Authorization", $"MediaBrowser Client=\"jellyfin_blazor\", Device=\"Jellyfin Blazor WebClient\", DeviceId=\"{deviceID}\", Version=\"1.0.0\", Token=\"{accessToken}\"");

            var response =  await base.SendAsync(request, cancellationToken);
            return response;
        }

        public async Task<HttpResponseMessage> SendAnonymousAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
