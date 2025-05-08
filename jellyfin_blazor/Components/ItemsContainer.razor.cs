using Microsoft.AspNetCore.Components;
using jellyfin_blazor.Data;
using jellyfin_blazor.Services;
using jellyfin_blazor.Identity;
using System.Net;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace jellyfin_blazor.Components
{
    public partial class ItemsContainer
    {
        [Parameter]
        public ItemType ItemType { get; set; }
        private List<dynamic> Items { get; set; } = new List<dynamic>();
        private HttpService _httpService;
        private IdentityContext _identityContext;

        public ItemsContainer(HttpService httpService, IdentityContext identity)
        {
            _httpService = httpService;
            _identityContext = identity;
        }
        protected override async Task OnInitializedAsync()
        {
            if (await GetItems())
            {
                StateHasChanged();
            }
        }

        private async Task<bool> GetItems()
        {
            string itemtypestring;

            switch (ItemType)
            {
                case ItemType.Movie:
                    itemtypestring = "Movie";
                    break;
                case ItemType.Series:
                    itemtypestring = "Series";
                    break;
                default:
                    return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"/Users/{_identityContext.Identity.Id}/Items?Recursive=true&IncludeItemTypes={itemtypestring}&Limit=10");

            var response = await _httpService.SendAsync(request, CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var _data = JsonConvert.DeserializeObject<ItemData>(content);
                Items = _data.Items;
                return true;
            }
            else
            {
                Console.WriteLine("Error fetching items: " + response.StatusCode);
                return true;
            }

        }
    }
}
